using System.Text.Json.Serialization;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.State;

internal sealed class LoadMessage
{
    [JsonPropertyName("loadId")]
    public string? LoadId { get; set; } = string.Empty;
    
    [JsonPropertyName("loadType")]
    public string? LoadType { get; set; }
    
    [JsonPropertyName("loadPosition")]
    public string? LoadPosition { get; set; }
    
    [JsonPropertyName("weight")]
    public double? Weight { get; set; }
    
    [JsonPropertyName("boundingBoxReference")]
    public BoundingBoxReferenceMessage? BoundingBoxReference { get; set; }
    
    [JsonPropertyName("dimensions")]
    public LoadDimensionsMessage? Dimensions { get; set; }
}