using System.Text.Json.Serialization;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.State;

internal class BatteryStateMessage
{
    [JsonPropertyName("batteryCharge")]
    public double BatteryCharge { get; set; }

    [JsonPropertyName("charging")]
    public bool Charging { get; set; }
    
    [JsonPropertyName("batteryVoltage")]
    public double? BatteryVoltage { get; set; }
    
    [JsonPropertyName("batteryHealth")]
    public int? BatteryHealth { get; set; }
    
    [JsonPropertyName("reach")]
    public uint? Reach { get; set; }
}