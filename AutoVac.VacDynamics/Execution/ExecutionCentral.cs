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

public partial class ExecutionCentral
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
      ExecuteCommands(instructions, state);
    }
    catch(BatteryExhaustionException) { }
    catch(AutoVacStuckException) { }

    PositionComparer? positionComparer = new ();

    return new ExecutionResult
    (
      Visited: new VirtualTrajectory(state.Visited.ToOrdererArray(positionComparer)),
      Cleaned: new VirtualTrajectory(state.Cleaned.ToOrdererArray(positionComparer)),
      Final: new OrientedPosition(state.Position, state.Facing),
      Battery: state.BatteryLevel
    );
  }

  private void ExecuteCommands(InstructionSequence instructions, VacState vacState)
  {
    ref ushort batteryLevel           = ref vacState.BatteryLevel;
    ref CardinalDirection orientation = ref vacState.Facing;

    int count = instructions.Count;
    for(int i = 0;i < count;++i)
    {
      InstructionKind instruction = instructions[i];

      DiagnosticLogger.LogInstruction(instruction);

      if(!BatteryProcessor.AdjustBatteryLevel(instruction, ref batteryLevel))
        throw new BatteryExhaustionException();

      if(OrientationProcessor.Orientate(ref orientation, instruction))
        continue;

      Position position = vacState.Position;
      if(instruction is InstructionKind.Clean)
      {
        vacState.AddCleaned(position); // imagine clean processor here
        continue;
      }

      GroundSection nextSection = TryMoveToNextSection(orientation, instruction, ref position);

      if(nextSection is not GroundSection.Space) // obstacle
        TryToBackOff(ref batteryLevel, ref orientation, ref position);

      vacState.Position = position; // Moved in either way. Continue with instruction sequence.
    }
  }

  private void TryToBackOff(ref ushort batteryLevel, ref CardinalDirection orientation, ref Position position)
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

        if(!BatteryProcessor.AdjustBatteryLevel(instruction, ref batteryLevel))
          throw new BatteryExhaustionException();

        if(OrientationProcessor.Orientate(ref orientation, instruction))
          continue;

        if(TryMoveToNextSection(orientation, instruction, ref position) == GroundSection.Space)
        {
          attemptSuccessful = true;
          continue;
        }

        break; // obstacle was hit
      }

      if(attemptSuccessful)
        return; // some of attempts succeeded in whole
    }

    throw new AutoVacStuckException(); // stuck
  }
}
