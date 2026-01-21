using System.Text.Json.Serialization;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.State;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.Visualization;

internal class VisualizationMessage
{
    [JsonPropertyName("agvPosition")]
    public AgvPosition AgvPosition { get; set; }

    [JsonPropertyName("velocity")]
    public Velocity? Velocity { get; set; }
}