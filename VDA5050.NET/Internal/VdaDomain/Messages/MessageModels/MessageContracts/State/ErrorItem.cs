using System.Text.Json.Serialization;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.State.Enums;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.State;

internal class ErrorItem
{
    [JsonPropertyName("errorType")]
    public string ErrorType { get; set; } = string.Empty;

    [JsonPropertyName("errorLevel")]
    public ErrorLevel ErrorLevel { get; set; }

    // maybe description or hint fields
}