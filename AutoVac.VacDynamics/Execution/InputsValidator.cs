using AutoVac.Clientship.InstructionSet;
using AutoVac.Clientship.Spatial;

using System.Diagnostics;

namespace AutoVac.VacDynamics.Execution;

internal static class InputsValidator
{
  [Conditional("UNCONTROLLED_ENVIRONMENT")]
  public static void GroundPlan(IReadOnlyList<IReadOnlyList<GroundSection>?>? groundPlan)
  {
    if(groundPlan is null)
      throw new ArgumentNullException(paramName: nameof(groundPlan), "Ground plan is void value!");

    int rowCount = groundPlan.Count;
    for(int rowIndex = 0;rowIndex < rowCount;++rowIndex)
    {
      IReadOnlyList<GroundSection>? row = groundPlan[rowIndex];
      if(row is null)
        throw new ArgumentException(message: $"Ground plan row at index {rowIndex} is void value!");

      int sectionCount = row.Count;
      for(int sectionIndex = 0;sectionIndex < sectionCount;++sectionIndex)
      {
        GroundSection section = row[sectionIndex];
        if(!Enum.IsDefined(section))
          throw new ArgumentException(message: $"Invalid section at {{X: {sectionIndex}; Y : {rowIndex}}}!");
      }
    }
  }

  [Conditional("UNCONTROLLED_ENVIRONMENT")]
  public static void Position(OrientedPosition orientedPosition, GroundPlan groundPlan)
  {
    CardinalDirection facing = orientedPosition.Facing;
    if(!Enum.IsDefined(facing))
      throw new ArgumentException(message: $"Invalid cardinal direction {facing}!");

    ushort
      x = orientedPosition.X,
      y = orientedPosition.Y;

    if(y < groundPlan.Count)
    {
      GroundPlanRow row = groundPlan[y];
      if(x < row.Count)
        if(row[x] is GroundSection.Space)
          return;
    }

    throw new ArgumentException(message: "AutoVac start position is at impossible place!");
  }

  [Conditional("UNCONTROLLED_ENVIRONMENT")]
  public static void Instructions(IReadOnlyList<InstructionKind>? instructions)
  {
    if(instructions is null)
      throw new ArgumentNullException(paramName: nameof(instructions), "Instructions is void value!");

    int count = instructions.Count;
    for(int i = 0;i < count;++i)
    {
      InstructionKind instr = instructions[i];
      if(!Enum.IsDefined(instr))
        throw new ArgumentException(message: $"Invalid instruction {instr} at index {i}!");

    }
  }
}
