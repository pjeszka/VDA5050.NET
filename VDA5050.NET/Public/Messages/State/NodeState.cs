using System.Text.Json.Serialization;

namespace VDA5050.NET.Public.Messages.State;

public class NodeState
{
    [JsonPropertyName("nodeId")]
    public string NodeId { get; set; } = string.Empty;

    [JsonPropertyName("sequenceId")]
    public uint SequenceId { get; set; }

    [JsonPropertyName("released")]
    public bool? Released { get; set; }

    // optionals like position etc
}