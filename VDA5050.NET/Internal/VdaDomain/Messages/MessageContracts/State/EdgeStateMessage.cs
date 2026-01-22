using System.Text.Json.Serialization;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Order;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.State;

internal class EdgeStateMessage
{
    [JsonPropertyName("edgeId")]
    public string EdgeId { get; set; } = string.Empty;

    [JsonPropertyName("sequenceId")]
    public uint SequenceId { get; set; }

    [JsonPropertyName("released")]
    public bool Released { get; set; }
    
    [JsonPropertyName("trajectory")]
    public TrajectoryMessage? Trajectory { get; set; }
}