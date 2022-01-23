using Autovac.Console.InputModel.Namings;

using static Autovac.Console.InputModel.Serialization.RawExecutionSettings;

namespace Autovac.Console.InputModel.Validation;

internal sealed class OrientatedPositionValidator
{
  private readonly OrientationMapping orientationMapping = new ();

  public void Validate(StartPoint? input)
  {
    if(input is null)
      throw new ArgumentNullException(paramName: nameof(input), "Start position is void value!");

    string? facing = input.Facing;
    if(facing is null || !orientationMapping.Contains(facing))
      throw new ArgumentException(message: "Invalid cardinal direction!", nameof(input));

    ushort?
      column = input.X,
      row    = input.Y;

    if(column is null || row is null)
      throw new ArgumentException(message: "At least one value of coordinate is not provided!", nameof(input));
  }
}
