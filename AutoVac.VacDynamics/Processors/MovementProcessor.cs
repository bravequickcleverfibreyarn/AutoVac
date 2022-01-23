using AutoVac.Clientship.Spatial;
using AutoVac.VacDynamics.ControlFlowManagement;

using static AutoVac.Clientship.Spatial.CardinalDirection;

using DirectionMasking = AutoVac.VacDynamics.ControlFlowManagement.ConjunctionMatchMasks.CardinalDirection;

namespace AutoVac.VacDynamics.Processors;

internal class MovementProcessor
{
  private readonly ZeroIndexStepModuloComplement directionComplement;
  private readonly GroundPlan groundPlan;
  public MovementProcessor(GroundPlan groundPlan)
  {
    this.groundPlan = groundPlan;

    // 2-step gap between cardinal direction complements, 4 cardinal directions
    directionComplement = new ZeroIndexStepModuloComplement(2, 4);
  }

  public GroundSection Advance(CardinalDirection facing, ref Position positon) => Move(facing, ref positon);

  public GroundSection Back(CardinalDirection facing, ref Position positon)
    => Move((CardinalDirection) directionComplement.GetComplement((int) facing), ref positon);

  private GroundSection Move(CardinalDirection direction, ref Position positon)
  {
    int
      xAxisIndex, // column
      yAxisIndex; // row

    // little masking perfomance
    if(((int) direction & DirectionMasking.YAxisMask) == DirectionMasking.YAxisMaskResult) // Y axis
    {
      int delta = direction switch
      {
        North => -1,
        South => +1,
        _=> throw new ArgumentOutOfRangeException(nameof(direction), actualValue: direction, null)
      };

      xAxisIndex = positon.X;
      yAxisIndex = positon.Y + delta;
    }
    else
    {
      int delta = direction switch
      {
        West => -1,
        East => +1,
        _=> throw new ArgumentOutOfRangeException(nameof(direction), actualValue: direction, null)
      };

      xAxisIndex = positon.X + delta;
      yAxisIndex = positon.Y;
    }

    GroundSection nextPosition = NextPosition(columnIndex: xAxisIndex, rowIndex: yAxisIndex);
    if(nextPosition is GroundSection.Space)
      positon = new Position((ushort) xAxisIndex, (ushort) yAxisIndex);

    return nextPosition;
  }

  private GroundSection NextPosition(int columnIndex, int rowIndex)
  {
    if(rowIndex < groundPlan.Count)
    {
      GroundPlanRow row = groundPlan[rowIndex];
      if(columnIndex < row.Count)
        return row[columnIndex];
    }

    return GroundSection.Wall;
  }
}
