namespace Autovac.Console.InputModel.Validation;

internal sealed class ValidationException : ArgumentException
{
  public ValidationException(string valueName, string message)
  : base(message: message)
  => ValueName = valueName;

  public ValidationException(string valueName, ArgumentException originalException)
  : base(originalException.Message, originalException)
    => ValueName = valueName;

  public string ValueName { get; }
}
