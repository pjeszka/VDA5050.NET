using System.Text.Json.Serialization;

namespace VDA5050.NET.Public.Messages;

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

    public void FillHeader(int headerId, string robotTopicPrefix, DateTime timeStamp)
    {
        HeaderId = headerId;
        Timestamp = timeStamp.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        var topicParts = robotTopicPrefix.Split('/');
        Version = "2.1.0";
        Manufacturer = topicParts[2];
        SerialNumber = topicParts[3];
    }
}
