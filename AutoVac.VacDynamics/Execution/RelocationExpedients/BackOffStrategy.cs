
using System.Collections.ObjectModel;

using static AutoVac.Clientship.InstructionSet.InstructionKind;

namespace AutoVac.VacDynamics.Execution.RelocationExpedients;

internal static class BackOffStrategy
{
  public static ReadOnlyCollection<InstructionSequence> Instructions => new
  (
    new InstructionSequence[]
    {
      new InstructionSequence(new [] { TurnRight, Advance, TurnLeft }),
      new InstructionSequence(new [] { TurnRight, Advance, TurnRight }),
      new InstructionSequence(new [] { TurnRight, Advance, TurnRight }),
      new InstructionSequence(new [] { TurnRight, Back, TurnRight, Advance }),
      new InstructionSequence(new [] { TurnLeft, TurnLeft, Advance }),
    }
  );
}
