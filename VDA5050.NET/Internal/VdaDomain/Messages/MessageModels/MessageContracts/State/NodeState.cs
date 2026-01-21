using System.Text.Json.Serialization;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.Order;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.State;

internal class NodeState
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