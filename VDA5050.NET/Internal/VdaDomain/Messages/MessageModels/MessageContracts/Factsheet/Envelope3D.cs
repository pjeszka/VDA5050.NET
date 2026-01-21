using System.Text.Json.Serialization;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.Factsheet;

internal class Envelope3D
{
    [JsonPropertyName("set")]
    public string Set { get; set; } = string.Empty;

    [JsonPropertyName("maxWeight")]
    public double? MaxWeight { get; set; }

    [JsonPropertyName("minLoadhandlingHeight")]
    public double? MinLoadhandlingHeight { get; set; }

    // etc
}