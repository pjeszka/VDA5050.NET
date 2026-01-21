using System.Text.Json.Serialization;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.State;

internal class AgvPosition
{
    [JsonPropertyName("x")]
    public double X { get; set; }

    [JsonPropertyName("y")]
    public double Y { get; set; }

    [JsonPropertyName("theta")]
    public double Theta { get; set; }

    [JsonPropertyName("positionInitialized")]
    public bool PositionInitialized { get; set; }

    [JsonPropertyName("mapId")]
    public string? MapId { get; set; }
}