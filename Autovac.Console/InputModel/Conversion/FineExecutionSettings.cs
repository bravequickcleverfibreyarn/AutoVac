
using AutoVac.Clientship.InstructionSet;
using AutoVac.Clientship.Spatial;

namespace Autovac.Console.InputModel.Conversion;

internal record class FineExecutionSettings
{
  public readonly GroundSection[][] GroundPlan;
  public readonly OrientedPosition Position;
  public readonly InstructionKind[] InstructionSequence;
  public readonly ushort BatteryLevel;

  public FineExecutionSettings(
    GroundSection[][] groundPlan,
    OrientedPosition position,
    InstructionKind[] instructionSequence,
    ushort batteryLevel
  )
  {
    GroundPlan = groundPlan;
    Position = position;
    InstructionSequence = instructionSequence;
    BatteryLevel = batteryLevel;
  }
}
