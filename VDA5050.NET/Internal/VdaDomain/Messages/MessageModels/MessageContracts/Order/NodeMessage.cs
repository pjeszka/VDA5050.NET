using System.Text.Json.Serialization;
using VDA5050.NET.Public.Models.Orders;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.Order;

internal class NodeMessage
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
    
    public static NodeMessage CreateNodeMessage(NodeDto nodeDto, uint index)
    {
        return new NodeMessage { 
            NodeId = nodeDto.Id, 
            SequenceId = index,
            NodeDescription = nodeDto.NodeDescription,
            Released = nodeDto.Released,
            NodePosition = nodeDto.NodePosition,
            Actions = nodeDto.Actions.Select(ActionItem.CreateActionItem).ToList()
        };
    }
}