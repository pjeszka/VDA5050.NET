using System.Text.Json.Serialization;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.Connection.Enums;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.Connection;

internal class ConnectionMessage : Header
{
    [JsonPropertyName("connectionState")]
    public ConnectionState ConnectionState { get; set; }
}