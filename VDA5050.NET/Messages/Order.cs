using System.Text.Json.Serialization;

namespace VDA5050.NET.Messages;

public class Order : Header
{
    [JsonPropertyName("orderId")]
    public string OrderId { get; set; } = default!;

    [JsonPropertyName("orderUpdateId")]
    public uint OrderUpdateId { get; set; }

    [JsonPropertyName("zoneSetId")]
    public string? ZoneSetId { get; set; }

    [JsonPropertyName("nodes")]
    public List<Node> Nodes { get; set; } = new();

    [JsonPropertyName("edges")]
    public List<Edge> Edges { get; set; } = new();
}
