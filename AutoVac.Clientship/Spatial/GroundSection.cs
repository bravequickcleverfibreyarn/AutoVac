namespace AutoVac.Clientship.Spatial;

public enum GroundSection : byte
{
  /// <summary>
  /// Occupiable and cleanable space.
  /// </summary>
  Space = 0,

  // obstacles

  /// <summary>
  /// Inaccessable space.
  /// </summary>
  Wall = 1,
  /// <summary>
  /// Obstacle-column.
  /// </summary>
  Column = 2,

}
