using System.Text.Json.Serialization;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.State;

internal class InfoReferenceMessage
{
    [JsonPropertyName("referenceKey")]
    public string ReferenceKey { get; set; } = string.Empty;
    
    [JsonPropertyName("referenceValue")]
    public string ReferenceValue{ get; set; } = string.Empty;
}