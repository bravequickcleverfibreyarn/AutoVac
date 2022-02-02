using AutoVac.Clientship;
using AutoVac.Clientship.InstructionSet;
using AutoVac.Clientship.Spatial;
using AutoVac.VacDynamics.ControlFlowManagement;
using AutoVac.VacDynamics.DiagnosticLogging;
using AutoVac.VacDynamics.Execution.RelocationExpedients;
using AutoVac.VacDynamics.Extensions;
using AutoVac.VacDynamics.Processors;
using AutoVac.VacDynamics.RelationalComparison;

using System.Collections.ObjectModel;
using System.Reflection;

namespace AutoVac.VacDynamics.Execution;

public sealed partial class ExecutionCentral
{

  private readonly MovementProcessor movementProcessor;

  public ExecutionCentral(GroundSection[][] groundPlan)
  {
    InputsValidator.GroundPlan(groundPlan);
    movementProcessor = new MovementProcessor(new GroundPlan(groundPlan.ToArray(x => new GroundPlanRow(x))));
  }

  public ExecutionResult Execute(OrientedPosition startPosition, InstructionKind[] instructionSequence, ushort batteryLevel)
  {
    InputsValidator.Position
    (
      startPosition,
      (GroundPlan) movementProcessor.GetType().GetField("groundPlan", BindingFlags.NonPublic | BindingFlags.Instance)!.GetValue(movementProcessor)!
    );

    InputsValidator.Instructions(instructionSequence);

    InstructionSequence instructions = new (instructionSequence);
    VacState state = PrepareState(startPosition, instructions, batteryLevel);

    try
    {
      ExecuteCommands(instructions, ref state);
    }
    catch(BatteryExhaustionException) { }
    catch(AutoVacStuckException) { }

    PositionComparer? positionComparer = new ();

    return new ExecutionResult
    (
      Visited: new VirtualTrajectory(state.Visited.ToOrderedArray(positionComparer)),
      Cleaned: new VirtualTrajectory(state.Cleaned.ToOrderedArray(positionComparer)),
      Final: new OrientedPosition(state.Position, state.Facing),
      Battery: state.BatteryLevel
    );
  }

  private void ExecuteCommands(InstructionSequence instructions, ref VacState vacState)
  {
    int count = instructions.Count;
    for(int i = 0;i < count;++i)
    {
      InstructionKind instruction = instructions[i];
      DiagnosticLogger.LogInstruction(instruction);

      if(ExecuteCommand(ref vacState, instruction) is false)
        TryToBackOff(ref vacState);
    }
  }

  private bool? ExecuteCommand(ref VacState vacState, InstructionKind instruction)
  {
    ushort batteryLevel = vacState.BatteryLevel;
    if(!BatteryProcessor.AdjustBatteryLevel(instruction, ref batteryLevel))
      throw new BatteryExhaustionException();
    vacState.BatteryLevel = batteryLevel;

    CardinalDirection orientation = vacState.Facing;
    if(OrientationProcessor.Orientate(ref orientation, instruction))
    {
      vacState.Facing = orientation;
      return null;
    }

    Position position = vacState.Position;
    if(instruction is InstructionKind.Clean)
    {
      vacState.AddCleaned(position); // imagine clean processor here
      return null;
    }

    GroundSection nextSection = TryMoveToNextSection(orientation, instruction, ref position);
    if(nextSection is GroundSection.Space)
    {
      vacState.Position = position;
      return true;
    }

    return false;
  }

  private void TryToBackOff(ref VacState vacState)
  {
    ReadOnlyCollection<InstructionSequence> backOffStrategy = BackOffStrategy.Instructions;

    int strategyCount = backOffStrategy.Count;
    for(int strategyIndex = 0;strategyIndex < strategyCount;++strategyIndex)
    {
      InstructionSequence currentAttempt = backOffStrategy[strategyIndex];
      DiagnosticLogger.Log(currentAttempt);

      bool attemptSuccessful = false;

      int instructionCount = currentAttempt.Count;
      for(int instructionIndex = 0;instructionIndex < instructionCount;++instructionIndex)
      {
        InstructionKind instruction = currentAttempt[instructionIndex];
        DiagnosticLogger.LogBackOffInstruction(instruction);

        bool? result = ExecuteCommand(ref vacState, instruction);
        if(result is false)
          break; // obstacle was hit

        if(result is true)
          attemptSuccessful = true;
      }

      if(attemptSuccessful)
        return; // some of attempts succeeded in whole
    }

    throw new AutoVacStuckException(); // stuck
  }
}
