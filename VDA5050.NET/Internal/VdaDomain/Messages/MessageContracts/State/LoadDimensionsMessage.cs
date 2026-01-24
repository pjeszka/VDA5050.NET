using System.Text.Json.Serialization;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.State;

internal sealed class LoadDimensionsMessage
{
    [JsonPropertyName("length")]
    public double Length { get; set; }

    [JsonPropertyName("width")]
    public double Width { get; set; }

    [JsonPropertyName("height")]
    public double? Height { get; set; }
}