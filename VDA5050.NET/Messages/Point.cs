using System.Text.Json.Serialization;

namespace VDA5050.NET.Messages;

public class Point
{
    [JsonPropertyName("x")]
    public double X { get; set; }

    [JsonPropertyName("y")]
    public double Y { get; set; }

    [JsonPropertyName("z")]
    public double Z { get; set; }

    [JsonPropertyName("t")]
    public double? T { get; set; }
}