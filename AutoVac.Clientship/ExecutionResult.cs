using AutoVac.Clientship.Spatial;

using System.Collections.ObjectModel;

namespace AutoVac.Clientship;

public sealed record class ExecutionResult
(
  ReadOnlyCollection<Position> Visited,
  ReadOnlyCollection<Position> Cleaned,
  OrientedPosition Final,
  ushort Battery
)
{
  /// <summary>
  /// Set of cells that were visited. 
  /// </summary>  
  public readonly ReadOnlyCollection<Position> Visited = Visited;

  /// <summary>
  /// Set of cells that were cleaned.
  /// </summary>
  public readonly ReadOnlyCollection<Position> Cleaned = Cleaned;

  /// <summary>
  /// Position where vac were upon execution end.
  /// </summary>
  public readonly OrientedPosition Final = Final;

  /// <summary>
  /// Final battery level.
  /// </summary>
  public readonly ushort Battery = Battery;
}
