
using System.Text.Json.Serialization;

namespace Autovac.Console.InputModel.Serialization;

internal record class RawExecutionSettings
{
  /// <summary>
  /// Room map represented in rows array.
  /// </summary>
  public readonly string?[]?[]? Map;
  public readonly StartPoint? Start;
  /// <summary>
  /// Command sequence.
  /// </summary>
  public readonly string?[]? Commands;
  /// <summary>
  /// Initial battery level.
  /// </summary>
  public readonly ushort? Battery;

  [JsonConstructor]
  public RawExecutionSettings
  (
    string?[]?[]? map,
    StartPoint? start,
    string?[]? commands,
    ushort? battery
  )
  {
    Map = map;
    Start = start;
    Commands = commands;
    Battery = battery;
  }

  /// <summary>
  /// Encapsulates start point data.
  /// </summary>
  public record class StartPoint
  {
    // starting point
    public readonly ushort? X;
    public readonly ushort? Y;
    // heading, nose orientation
    public readonly string? Facing;

    [JsonConstructor]
    public StartPoint(ushort? x, ushort? y, string? facing)
    {
      X = x;
      Y = y;
      Facing = facing;
    }
  }
}
