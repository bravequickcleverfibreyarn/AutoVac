using AutoVac.Clientship.Spatial;

using System.Diagnostics;

namespace AutoVac.VacDynamics.Execution;

[DebuggerDisplay("Facing = {Facing}|Battery = {BatteryLevel}|Position = {Position}")]
internal struct VacState
{
  private readonly HashSet<Position> visited;
  private readonly HashSet<Position> cleaned;

  private Position position;

  public VacState
  (
    int visitedCountGuess,    //min guess base od advance instruction count
    int expectedCleanedCount, // guess made by clean commands count (maximum)
    OrientedPosition orientedPosition,
    ushort batteryLevel
  )
  {
    visited = new HashSet<Position>(visitedCountGuess);
    cleaned = new HashSet<Position>(expectedCleanedCount);

    (Position position, CardinalDirection facing) = orientedPosition;

    Facing = facing;
    BatteryLevel = batteryLevel;

    this.position = position;
    _ = visited.Add(position);
  }

  public Position Position
  {
    get => position;
    set
    {
      _ = visited.Add(value);
      position = value;
    }
  }
  public CardinalDirection Facing { get; set; }
  public ushort BatteryLevel { get; set; }

  public void AddCleaned(Position position) => cleaned.Add(position);

  public IReadOnlySet<Position> Visited => visited;
  public IReadOnlySet<Position> Cleaned => cleaned;
}
