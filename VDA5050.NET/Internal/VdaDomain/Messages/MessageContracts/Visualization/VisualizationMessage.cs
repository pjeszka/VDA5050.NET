using System.Text.Json.Serialization;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.State;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Visualization;

internal class VisualizationMessage
{
    [JsonPropertyName("agvPosition")]
    public AgvPositionMessage AgvPositionMessage { get; set; }

    [JsonPropertyName("velocity")]
    public VelocityMessage? Velocity { get; set; }
}