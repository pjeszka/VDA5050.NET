using System.Text.Json.Serialization;

namespace VDA5050.NET.Internal.Messages;

public abstract class Header
{
    [JsonPropertyName("headerId")]
    public int HeaderId { get; set; } = default!;

    [JsonPropertyName("timestamp")]
    public string Timestamp { get; set; } = default!;

    [JsonPropertyName("version")]
    public string Version { get; set; }

    [JsonPropertyName("manufacturer")]
    public string Manufacturer { get; set; }
    
    [JsonPropertyName("serialNumber")]
    public string SerialNumber { get; set; }
}
