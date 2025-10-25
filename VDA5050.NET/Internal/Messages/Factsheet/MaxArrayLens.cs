using System.Text.Json.Serialization;

namespace VDA5050.NET.Internal.Messages.Factsheet;

public class MaxArrayLens
{
    [JsonPropertyName("order.nodes")]
    public uint? OrderNodes { get; set; }

    [JsonPropertyName("order.edges")]
    public uint? OrderEdges { get; set; }

    [JsonPropertyName("node.actions")]
    public uint? NodeActions { get; set; }

    [JsonPropertyName("edge.actions")]
    public uint? EdgeActions { get; set; }

    // more fields as needed, matching spec
}