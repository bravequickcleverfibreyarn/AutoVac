using AutoVac.Clientship.InstructionSet;

using static AutoVac.Clientship.InstructionSet.InstructionKind;
using static AutoVac.VacDynamics.ControlFlowManagement.ConjunctionMatchMasks.InstructionKind;

namespace AutoVac.VacDynamics.Processors;

internal static class BatteryProcessor
{ 
  public static bool AdjustBatteryLevel(InstructionKind instruction, ref ushort batteryLevel)
  {
    // little masking perfomance
    ushort energyDemand = ((int) instruction & TurnCommandMask) == TurnCommandMaskResult
      ? (ushort) 1
      : (ushort) (instruction switch
      {        
        Advance               /**/  => 2,
        Back                  /**/  => 3,
        Clean                 /**/  => 5,
        _ => throw new ArgumentOutOfRangeException(paramName: nameof(instruction), actualValue: instruction, null)
      });

    if(energyDemand <= batteryLevel)
    {
      batteryLevel -= energyDemand;
      return true;
    }

    return false;
  }
}
