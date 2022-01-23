using AutoVac.Clientship.InstructionSet;

using static AutoVac.Clientship.InstructionSet.InstructionKind;

namespace Autovac.Console.InputModel.Namings;

internal sealed class InstructionMapping
{
  private readonly IReadOnlyDictionary<string, InstructionKind> mapping = new Dictionary<string, InstructionKind>()
  {
    ["TR"]  = TurnRight,
    ["TL"]  = TurnLeft,
    ["A"]   = Advance,
    ["B"]   = Back,
    ["C"]   = Clean
  };

  public bool Contains(string name) => mapping.ContainsKey(name);
  public InstructionKind Get(string name) => mapping[name];
}
