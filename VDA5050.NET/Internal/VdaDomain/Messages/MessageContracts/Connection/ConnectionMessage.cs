using System.Text.Json.Serialization;
using VDA5050.NET.Public.Enums.Vda5050.Connection;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Connection;

internal class ConnectionMessage : Header
{
    [JsonPropertyName("connectionState")]
    public ConnectionState ConnectionState { get; set; }
}