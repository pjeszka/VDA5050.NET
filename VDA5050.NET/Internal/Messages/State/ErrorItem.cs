using System.Text.Json.Serialization;
using VDA5050.NET.Internal.Messages.State.Enums;

namespace VDA5050.NET.Internal.Messages.State;

public class ErrorItem
{
    [JsonPropertyName("errorType")]
    public string ErrorType { get; set; } = string.Empty;

    [JsonPropertyName("errorLevel")]
    public ErrorLevel ErrorLevel { get; set; }

    // maybe description or hint fields
}