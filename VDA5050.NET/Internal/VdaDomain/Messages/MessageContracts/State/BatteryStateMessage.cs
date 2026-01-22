using System.Text.Json.Serialization;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.State;

internal class BatteryStateMessage
{
    [JsonPropertyName("batteryCharge")]
    public double BatteryCharge { get; set; }

    [JsonPropertyName("charging")]
    public bool Charging { get; set; }
}