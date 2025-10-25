using System.Text.Json.Serialization;
using VDA5050.NET.Internal.Messages.State.Enums;

namespace VDA5050.NET.Internal.Messages.State;

public class ActionState
{
    [JsonPropertyName("actionId")]
    public string ActionId { get; set; } = string.Empty;

    [JsonPropertyName("actionStatus")]
    public ActionStatus ActionStatus { get; set; }

    [JsonPropertyName("blockingType")]
    public BlockingType? BlockingType { get; set; }

    [JsonPropertyName("resultDescription")]
    public string? ResultDescription { get; set; }

    // possible other fields depending on schema
}