using System.Text.Json.Serialization;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts;

internal abstract class Header
{
    [JsonPropertyName("headerId")]
    public uint HeaderId { get; set; } = default!;

    [JsonPropertyName("timestamp")]
    public string Timestamp { get; set; } = default!;

    [JsonPropertyName("version")]
    public string Version { get; set; }

    [JsonPropertyName("manufacturer")]
    public string Manufacturer { get; set; }
    
    [JsonPropertyName("serialNumber")]
    public string SerialNumber { get; set; }

    public void FillHeader(uint headerId, string robotTopicPrefix, DateTime timeStamp)
    {
        HeaderId = headerId;
        Timestamp = timeStamp.ToString("yyyy-MM-ddTHH:mm:ss.fffZ");
        var topicParts = robotTopicPrefix.Split('/');
        Version = "2.1.0";
        Manufacturer = topicParts[2];
        SerialNumber = topicParts[3];
    }
}
