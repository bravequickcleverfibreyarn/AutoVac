using Autovac.Console.InputModel.Namings;

using AutoVac.Clientship;
using AutoVac.Clientship.Spatial;

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Autovac.Console.InputModel.Serialization;

internal static class InputOutputSerializer
{
  public static RawExecutionSettings? Deserialize(string input)
  {
    JsonSerializerOptions options = new ()
    {      
      PropertyNameCaseInsensitive = true,
      IncludeFields = true
    };

    try
    {
      return JsonSerializer.Deserialize<RawExecutionSettings>(input, options);
    }
    catch
    {
      throw;
    }
  }

  public static string Serialize(ExecutionResult result)
  {
    JsonSerializerOptions options = new ()
    {
      PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
      IncludeFields = true,
      WriteIndented = true, // produces highly intented and structured output
      Converters = { new FacingConverter() }
    };

    try
    {
      return JsonSerializer.Serialize(result, options);
    }
    catch
    {
      throw;
    }
  }

  private sealed class FacingConverter : JsonConverter<CardinalDirection>
  {
    private readonly OrientationMapping orientationMapping;
    private readonly Type isConverting;

    public FacingConverter()
    {
      orientationMapping = new OrientationMapping();
      isConverting = typeof(CardinalDirection);
    }

    public override bool CanConvert(Type typeToConvert) => isConverting == typeToConvert;

    public override CardinalDirection Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
      => throw new NotSupportedException();

    public override void Write(Utf8JsonWriter writer, CardinalDirection value, JsonSerializerOptions options)
      => writer.WriteStringValue(orientationMapping.Get(value));
  }
}
