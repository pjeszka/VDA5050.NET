using System.Text.Json.Serialization;

namespace VDA5050.NET.Messages;

public class ActionItem
{
    [JsonPropertyName("actionId")]
    public string ActionId { get; set; } = default!;

    [JsonPropertyName("actionType")]
    public string ActionType { get; set; } = default!;

    [JsonPropertyName("blockingType")]
    public string BlockingType { get; set; } = default!;

    [JsonPropertyName("actionParameters")]
    public List<Parameter> ActionParameters { get; set; } = new();
}