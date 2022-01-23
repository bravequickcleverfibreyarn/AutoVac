using AutoVac.Clientship.Spatial;

using KeyValuePair = System.Collections.Generic.KeyValuePair<string, AutoVac.Clientship.Spatial.CardinalDirection>;
using static AutoVac.Clientship.Spatial.CardinalDirection;

namespace Autovac.Console.InputModel.Namings;

internal sealed class OrientationMapping
{
  private readonly IReadOnlyDictionary<string, CardinalDirection> mapping = new Dictionary<string, CardinalDirection>()
  {
    ["N"] = North,
    ["S"] = South,
    ["W"] = West,
    ["E"] = East,
  };

  public bool Contains(string name) => mapping.ContainsKey(name);
  public CardinalDirection Get(string name) => mapping[name];
  public string Get(CardinalDirection cd)
  {
    foreach(KeyValuePair kv in mapping)
      if(kv.Value == cd)
        return kv.Key;

    throw new ArgumentOutOfRangeException(paramName: nameof(cd), actualValue: cd, "Unsupported direction!");
  }
}
