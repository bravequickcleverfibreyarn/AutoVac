namespace AutoVac.Clientship.Spatial;

public record struct OrientedPosition(Position Position, CardinalDirection Facing)
{
  private readonly Position Position = Position;

  public readonly CardinalDirection Facing = Facing;
  public ushort X => Position.X;
  public ushort Y => Position.Y;
}
