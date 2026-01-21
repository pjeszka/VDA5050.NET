using System.Text.Json.Serialization;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.State;

internal class BatteryState
{
    [JsonPropertyName("batteryCharge")]
    public double BatteryCharge { get; set; }

    [JsonPropertyName("charging")]
    public bool Charging { get; set; }
}