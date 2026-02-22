using System.Text.Json.Serialization;
using VDA5050.NET.Public.Enums.Vda5050.Connection;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Connection;

internal class ConnectionMessage : Header
{
    [JsonPropertyName("connectionState")]
    public ConnectionState ConnectionState { get; set; }

    public static ConnectionMessage CreateConnectionBrokenMessage(
        uint headerId,
        string robotTopicPrefix,
        DateTime timestamp)
    {
        var message = new ConnectionMessage()
        {
            ConnectionState = ConnectionState.CONNECTIONBROKEN
        };
        
        message.FillHeader(headerId, robotTopicPrefix, timestamp);
        
        return message;
    }
    
    public static ConnectionMessage CreateOnlineMessage(
        uint headerId,
        string robotTopicPrefix,
        DateTime timestamp)
    {
        var message = new ConnectionMessage()
        {
            ConnectionState = ConnectionState.ONLINE
        };
        
        message.FillHeader(headerId, robotTopicPrefix, timestamp);
        
        return message;
    }
    
    public static ConnectionMessage CreateOfflineMessage(
        uint headerId,
        string robotTopicPrefix,
        DateTime timestamp)
    {
        var message = new ConnectionMessage()
        {
            ConnectionState = ConnectionState.OFFLINE
        };
        
        message.FillHeader(headerId, robotTopicPrefix, timestamp);
        
        return message;
    }
}