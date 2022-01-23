using Autovac.Console.Extensions;
using Autovac.Console.InputModel.Namings;
using Autovac.Console.InputModel.Serialization;

using AutoVac.Clientship.InstructionSet;
using AutoVac.Clientship.Spatial;

using static Autovac.Console.InputModel.Serialization.RawExecutionSettings;

namespace Autovac.Console.InputModel.Conversion;

internal class SettingsConvertor
{
  private readonly OrientationMapping orientationMapping;
  private readonly InstructionMapping instructionMapping;
  private readonly GroundSectionMapping groundSectionMapping;

  public SettingsConvertor()
  {
    orientationMapping = new OrientationMapping();
    instructionMapping = new InstructionMapping();
    groundSectionMapping = new GroundSectionMapping();
  }

  public FineExecutionSettings Convert(RawExecutionSettings settings)
  {
    InstructionKind[] instructionSequence = settings.Commands!.ToArray(x => instructionMapping.Get(x!));
    ushort batteryLevel                   = settings.Battery!.Value;
    GroundSection[][] groundPlan          = settings.Map!.ToArray(x => x!.ToArray(y => groundSectionMapping.Get(y)));

    OrientedPosition orientedPosition;
    {
      StartPoint start                = settings.Start!;
      Position position               = new (start.X!.Value, start.Y!.Value);
      CardinalDirection orientation   = orientationMapping.Get(start.Facing!);

      orientedPosition = new(position, orientation);
    }

    return new FineExecutionSettings(groundPlan, orientedPosition, instructionSequence, batteryLevel);
  }
}
