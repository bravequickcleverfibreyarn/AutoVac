using Autovac.Console.InputModel.Namings;

namespace Autovac.Console.InputModel.Validation;

internal sealed class GroundSectionsValidator
{
  private readonly GroundSectionMapping groundSectionMapping = new();

  public void Validate(IReadOnlyList<IReadOnlyList<string?>?>? input)
  {
    if(input is null)
      throw new ArgumentNullException(paramName: nameof(input), "No value provided for ground plan!");

    int rowIndex = -1;
    foreach(IReadOnlyList<string?>? row in input)
    {
      ++rowIndex;
      if(row is null)
        throw new ArgumentException(message: $"Ground plan row at index {rowIndex} is void value!", nameof(input));

      int columnIndex = -1;
      foreach(string? column in row)
      {
        ++columnIndex;
        if(!groundSectionMapping.Contains(column))
          throw new ArgumentException(message: $"Invalid column value at position {{{columnIndex}; {rowIndex}}}!", nameof(input));
      }
    }
  }
}
