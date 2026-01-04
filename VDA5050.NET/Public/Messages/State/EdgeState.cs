using System.Text.Json.Serialization;
using VDA5050.NET.Public.Messages.Order;

namespace VDA5050.NET.Public.Messages.State;

public class EdgeState
{
    [JsonPropertyName("edgeId")]
    public string EdgeId { get; set; } = string.Empty;

    [JsonPropertyName("sequenceId")]
    public uint SequenceId { get; set; }

    [JsonPropertyName("released")]
    public bool Released { get; set; }
    
    [JsonPropertyName("trajectory")]
    public Trajectory? Trajectory { get; set; }
}