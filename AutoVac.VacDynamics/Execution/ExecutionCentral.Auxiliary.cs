using AutoVac.Clientship.InstructionSet;
using AutoVac.Clientship.Spatial;

namespace AutoVac.VacDynamics.Execution;

public sealed partial class ExecutionCentral
{

  private GroundSection TryMoveToNextSection(CardinalDirection orientation, InstructionKind instruction, ref Position position)
  => instruction switch
  {
    InstructionKind.Advance => movementProcessor.Advance(orientation, ref position),
    InstructionKind.Back => movementProcessor.Back(orientation, ref position),
    _ => throw new ArgumentOutOfRangeException(paramName: nameof(instruction), actualValue: instruction, null)
  };

  private static VacState PrepareState(OrientedPosition orientedPosition, InstructionSequence instructionSequence, ushort batteryLevel)
  {
    int
      advances = 0,
      cleanes = 0;

    int count = instructionSequence.Count;
    for(int i = 0;i < count;++i)
    {
      InstructionKind command = instructionSequence[i];
      if(command is InstructionKind.Advance)
        ++advances;
      else if(command is InstructionKind.Clean)
        ++cleanes;
    }

    return new VacState
    (
      visitedCountGuess: advances + 1, // + 1 for start point
      expectedCleanedCount: cleanes,
      orientedPosition,
      batteryLevel
    );
  }
}
