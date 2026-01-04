using System.Text.Json.Serialization;
using VDA5050.NET.Public.Messages.Order;

namespace VDA5050.NET.Public.Messages.State;

public class NodeState
{
    [JsonPropertyName("nodeId")]
    public string NodeId { get; set; } = string.Empty;

    [JsonPropertyName("sequenceId")]
    public uint SequenceId { get; set; }
    
    [JsonPropertyName("nodeDescription")]
    public string? NodeDescription { get; set; }

    [JsonPropertyName("released")]
    public bool Released { get; set; }

    [JsonPropertyName("nodePosition")]
    public NodePosition? NodePosition { get; set; }
}