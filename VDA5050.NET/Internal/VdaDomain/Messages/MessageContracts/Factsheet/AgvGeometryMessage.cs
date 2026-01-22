using System.Text.Json.Serialization;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Factsheet;

internal class AgvGeometryMessage
{
    [JsonPropertyName("wheelDefinitions")]
    public List<WheelDefinitionMessage>? WheelDefinitions { get; set; }

    [JsonPropertyName("envelopes3d")]
    public List<Envelope3DMessage>? Envelopes3d { get; set; }

    // add others like envelopes2d etc
}