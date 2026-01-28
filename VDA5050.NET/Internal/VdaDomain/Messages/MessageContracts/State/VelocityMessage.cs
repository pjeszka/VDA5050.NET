using System.Text.Json.Serialization;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.State;

internal class VelocityMessage
{
    [JsonPropertyName("vx")]
    public double Vx { get; set; }

    [JsonPropertyName("vy")]
    public double Vy { get; set; }

    [JsonPropertyName("omega")]
    public double Omega { get; set; }
}