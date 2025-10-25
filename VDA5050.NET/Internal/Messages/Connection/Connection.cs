using System.Text.Json.Serialization;
using VDA5050.NET.Internal.Messages.Connection.Enums;

namespace VDA5050.NET.Internal.Messages.Connection;

public class Connection : Header
{
    [JsonPropertyName("connectionState")]
    public ConnectionState ConnectionState { get; set; }
}