using System.Text.Json.Serialization;

namespace VDA5050.NET.Internal.Messages.Factsheet;

public class AgvGeometry
{
    [JsonPropertyName("wheelDefinitions")]
    public List<WheelDefinition>? WheelDefinitions { get; set; }

    [JsonPropertyName("envelopes3d")]
    public List<Envelope3D>? Envelopes3d { get; set; }

    // add others like envelopes2d etc
}