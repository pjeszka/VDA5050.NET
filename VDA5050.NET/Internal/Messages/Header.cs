using System.Text.Json.Serialization;

namespace VDA5050.NET.Internal.Messages;

public class Header
{
    [JsonPropertyName("interfaceName")]
    public string InterfaceName { get; set; } = default!;

    [JsonPropertyName("interfaceVersion")]
    public string InterfaceVersion { get; set; } = default!;

    [JsonPropertyName("sender")]
    public Identity Sender { get; set; } = new();

    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }
}
