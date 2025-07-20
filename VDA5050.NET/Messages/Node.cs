using System.Text.Json.Serialization;

namespace VDA5050.NET.Messages;

public class Node
{
    [JsonPropertyName("nodeId")]
    public string NodeId { get; set; } = default!;

    [JsonPropertyName("sequenceId")]
    public uint SequenceId { get; set; }

    [JsonPropertyName("nodeDescription")]
    public string? NodeDescription { get; set; }

    [JsonPropertyName("released")]
    public bool Released { get; set; }

    [JsonPropertyName("nodePosition")]
    public NodePosition NodePosition { get; set; } = new();

    [JsonPropertyName("actions")]
    public List<ActionItem> Actions { get; set; } = new();
}