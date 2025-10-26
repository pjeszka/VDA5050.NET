using System.Text.Json.Serialization;
using VDA5050.NET.Public.Messages.Connection.Enums;

namespace VDA5050.NET.Public.Messages.Connection;

public class Connection : Header
{
    [JsonPropertyName("connectionState")]
    public ConnectionState ConnectionState { get; set; }
}