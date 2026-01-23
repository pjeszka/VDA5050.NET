using System.Text.Json.Serialization;
using VDA5050.NET.Public.Enums.Vda5050.Order;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Factsheet;

internal class WheelDefinitionMessage
{
    [JsonPropertyName("type")]
    public WheelType Type { get; set; }

    [JsonPropertyName("isActiveDriven")]
    public bool? IsActiveDriven { get; set; }

    [JsonPropertyName("envelope2d")]
    public List<Point2DMessage>? Envelope2d { get; set; }
}