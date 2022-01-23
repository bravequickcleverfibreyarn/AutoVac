using Autovac.Console.InputModel.Namings;

namespace Autovac.Console.InputModel.Validation;

internal class InstructionsValidator
{
  private readonly InstructionMapping instructionMapping = new();

  public void Validate(IReadOnlyList<string?>? input)
  {
    if(input is null)
      throw new ArgumentNullException(paramName: nameof(input), "Instructions is void value!");

    int index = -1;
    foreach(string? value in input)
    {
      ++index;
      if(value is null || !instructionMapping.Contains(value))
        throw new ArgumentException(message: $"Invalid instruction at index {index}!", nameof(input));
    }
  }
}
