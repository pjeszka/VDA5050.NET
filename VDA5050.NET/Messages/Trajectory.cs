using System.Text.Json.Serialization;

namespace VDA5050.NET.Messages;

public class Trajectory
{
    [JsonPropertyName("type")]
    public string Type { get; set; } = default!;

    [JsonPropertyName("degree")]
    public int Degree { get; set; }

    [JsonPropertyName("controlPoints")]
    public List<Point> ControlPoints { get; set; } = new();
}