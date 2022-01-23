using AutoVac.Clientship.Spatial;

using static AutoVac.Clientship.Spatial.GroundSection;

namespace Autovac.Console.InputModel.Namings;

internal sealed class GroundSectionMapping
{
  private const string @null = "null";

  private readonly IReadOnlyDictionary<string, GroundSection> mapping = new Dictionary<string, GroundSection>()
  {
    ["S"]  = Space,
    ["C"]  = Column,
    [@null] = Wall,
  };

  public bool Contains(string? name) => mapping.ContainsKey(Key(name));
  public GroundSection Get(string? name) => mapping[Key(name)];
  private static string Key(string? name) => name ?? @null;
}
