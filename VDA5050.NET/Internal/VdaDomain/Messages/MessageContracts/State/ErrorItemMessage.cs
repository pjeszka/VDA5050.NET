using System.Text.Json.Serialization;
using VDA5050.NET.Public.Enums.Vda5050.State;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.State;

internal class ErrorItemMessage
{
    [JsonPropertyName("errorType")]
    public string ErrorType { get; set; } = string.Empty;

    [JsonPropertyName("errorLevel")]
    public ErrorLevel ErrorLevel { get; set; }
    
    [JsonPropertyName("errorReferences")]
    public ErrorReferenceMessage[]? ErrorReferences { get; set; }

    [JsonPropertyName("errorDescription")]
    public string? ErrorDescription { get; set; } = null;
    
    [JsonPropertyName("errorHint")]
    public string? ErrorHint { get; set; } = null;
}

internal class ErrorReferenceMessage
{
    [JsonPropertyName("referenceKey")]
    public string ReferenceKey { get; set; } = string.Empty;
    
    [JsonPropertyName("referenceValue")]
    public string ReferenceValue { get; set; } = string.Empty;
}