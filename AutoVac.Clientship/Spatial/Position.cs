namespace AutoVac.Clientship.Spatial;

public record struct Position(ushort X, ushort Y) : IComparable<Position>
{
  public readonly ushort X = X;
  public readonly ushort Y = Y;

  public int CompareTo(Position other)
  {
    Comparer<int> defaultIntComparer = Comparer<int>.Default;

    int yComparision = defaultIntComparer.Compare(Y, other.Y);

    return yComparision is 0
      ? defaultIntComparer.Compare(X, other.X)
      : yComparision;
  }
}
