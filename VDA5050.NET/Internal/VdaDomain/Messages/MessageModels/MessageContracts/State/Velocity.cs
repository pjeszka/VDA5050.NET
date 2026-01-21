using System.Text.Json.Serialization;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.State;

internal class Velocity
{
    [JsonPropertyName("vx")]
    public double Vx { get; set; }

    [JsonPropertyName("vy")]
    public double Vy { get; set; }

    [JsonPropertyName("omega")]
    public double Omega { get; set; }
}