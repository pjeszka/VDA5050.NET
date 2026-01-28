using System.Text.Json.Serialization;
using VDA5050.NET.Public.Enums.Vda5050.State;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.State;

internal sealed class InformationItemMessage
{
    [JsonPropertyName("infoType")]
    public string InfoType { get; set; } = string.Empty;
    
    [JsonPropertyName("infoDescription")]
    public string? InfoDescription { get; set; } = string.Empty;
    
    [JsonPropertyName("infoLevel")]
    public InfoLevel InfoLevel { get; set; }
    
    [JsonPropertyName("infoReferences")]
    public ICollection<InfoReferenceMessage>? InfoReferences { get; set; }
}