using System.Text.Json.Serialization;

namespace VDA5050.NET.Messages;

public class NodePosition
{
    [JsonPropertyName("x")]
    public double X { get; set; }

    [JsonPropertyName("y")]
    public double Y { get; set; }

    [JsonPropertyName("z")]
    public double Z { get; set; }

    [JsonPropertyName("theta")]
    public double? Theta { get; set; }

    [JsonPropertyName("mapId")]
    public string MapId { get; set; } = default!;
}
