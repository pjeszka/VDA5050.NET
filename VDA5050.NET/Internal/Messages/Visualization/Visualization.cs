using System.Text.Json.Serialization;
using VDA5050.NET.Internal.Messages.State;

namespace VDA5050.NET.Internal.Messages.Visualization;

public class Visualization
{
    [JsonPropertyName("agvPosition")]
    public AgvPosition AgvPosition { get; set; }

    [JsonPropertyName("velocity")]
    public Velocity? Velocity { get; set; }
}