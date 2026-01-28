using System.Text.Json.Serialization;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.State;

internal class BoundingBoxReferenceMessage
{
    [JsonPropertyName("x")]
    public double X { get; set; }
    
    [JsonPropertyName("y")]
    public double Y { get; set; }
    
    [JsonPropertyName("z")]
    public double Z { get; set; }
    
    [JsonPropertyName("theta")]
    public double? Theta { get; set; }
}