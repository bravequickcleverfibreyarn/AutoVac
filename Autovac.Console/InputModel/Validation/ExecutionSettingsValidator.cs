using Autovac.Console.InputModel.Serialization;

namespace Autovac.Console.InputModel.Validation;

internal class ExecutionSettingsValidator
{
  readonly GroundSectionsValidator groundSections;
  readonly InstructionsValidator instructions;
  readonly OrientatedPositionValidator orientatedPosition;

  public ExecutionSettingsValidator()
  {
    groundSections = new GroundSectionsValidator();
    instructions = new InstructionsValidator();
    orientatedPosition = new OrientatedPositionValidator();
  }

  public void Validate(RawExecutionSettings settings)
  {
    if(settings.Battery is null)
      throw new ValidationException(valueName: nameof(settings.Battery).ToLowerInvariant(), "No value provided for battery level!");

    try
    {
      groundSections.Validate(settings.Map);
    }
    catch(ArgumentException ae)
    {
      throw new ValidationException(nameof(settings.Map).ToLowerInvariant(), ae);
    }
    try
    {
      instructions.Validate(settings.Commands);
    }
    catch(ArgumentException ae)
    {
      throw new ValidationException(nameof(settings.Commands).ToLowerInvariant(), ae);
    }
    try
    {
      orientatedPosition.Validate(settings.Start);
    }
    catch(ArgumentException ae)
    {
      throw new ValidationException(nameof(settings.Start).ToLowerInvariant(), ae);
    }
  }
}
