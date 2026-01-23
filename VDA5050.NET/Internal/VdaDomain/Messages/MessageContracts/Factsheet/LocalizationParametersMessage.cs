using System.Text.Json.Serialization;
using VDA5050.NET.Public.Enums.Vda5050.Order;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Factsheet;

internal class LocalizationParametersMessage
{
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("type")]
    public LocalizationType? Type { get; set; }
}