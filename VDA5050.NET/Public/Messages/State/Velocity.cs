using System.Text.Json.Serialization;

namespace VDA5050.NET.Public.Messages.State;

public class Velocity
{
    [JsonPropertyName("vx")]
    public double Vx { get; set; }

    [JsonPropertyName("vy")]
    public double Vy { get; set; }

    [JsonPropertyName("omega")]
    public double Omega { get; set; }
}