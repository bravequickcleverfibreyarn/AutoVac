using AutoVac.Clientship.Spatial;

namespace AutoVac.VacDynamics.RelationalComparison;

internal sealed class PositionComparer : IComparer<Position>
{
  public int Compare(Position x, Position y) => x.CompareTo(y);
}
