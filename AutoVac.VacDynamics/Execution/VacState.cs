using AutoVac.Clientship.Spatial;

namespace AutoVac.VacDynamics.Execution;

internal class VacState
{
  private readonly HashSet<Position> visited;
  private readonly HashSet<Position> cleaned;

  private CardinalDirection facing;
  private Position position;
  private ushort batteryLevel;

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

    this.facing = facing;
    this.batteryLevel = batteryLevel;

    Position = position;
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
  public ref CardinalDirection Facing => ref facing;
  public ref ushort BatteryLevel => ref batteryLevel;

  public void AddCleaned(Position position) => cleaned.Add(position);

  public IReadOnlySet<Position> Visited => visited;
  public IReadOnlySet<Position> Cleaned => cleaned;
}
