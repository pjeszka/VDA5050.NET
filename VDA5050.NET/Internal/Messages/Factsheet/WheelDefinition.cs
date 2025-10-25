using System.Text.Json.Serialization;
using VDA5050.NET.Internal.Messages.Factsheet.Enums;

namespace VDA5050.NET.Internal.Messages.Factsheet;

public class WheelDefinition
{
    [JsonPropertyName("type")]
    public WheelType Type { get; set; }

    [JsonPropertyName("isActiveDriven")]
    public bool? IsActiveDriven { get; set; }

    [JsonPropertyName("envelope2d")]
    public List<Point2D>? Envelope2d { get; set; }
}