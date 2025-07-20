using System.Text.Json.Serialization;

namespace VDA5050.NET.Messages;

public class Identity
{
    [JsonPropertyName("manufacturer")]
    public string Manufacturer { get; set; } = default!;

    [JsonPropertyName("serialNumber")]
    public string SerialNumber { get; set; } = default!;
}