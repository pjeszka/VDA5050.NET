using System.Text.Json.Serialization;

namespace VDA5050.NET.Public.Messages.Factsheet;

public class Point2D
{
    [JsonPropertyName("x")]
    public double X { get; set; }

    [JsonPropertyName("y")]
    public double Y { get; set; }
}