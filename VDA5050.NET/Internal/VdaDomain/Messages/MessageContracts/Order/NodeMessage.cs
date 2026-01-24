using System.Text.Json.Serialization;
using VDA5050.NET.Public.Models.Orders;
using VDA5050.NET.Public.Models.Orders.OrderRequesting;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Order;

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
    public NodePositionMessage NodePositionMessage { get; set; } = new();

    [JsonPropertyName("actions")]
    public List<ActionItemMessage> Actions { get; set; } = new();
    
    public static NodeMessage CreateNodeMessage(Node node, uint index)
    {
        return new NodeMessage { 
            NodeId = node.Id, 
            SequenceId = index,
            NodeDescription = node.NodeDescription,
            Released = node.Released,
            NodePositionMessage = NodePositionMessage.CreateMessage(node.NodePose),
            Actions = node.Actions.Select(ActionItemMessage.CreateActionItem).ToList()
        };
    }
}