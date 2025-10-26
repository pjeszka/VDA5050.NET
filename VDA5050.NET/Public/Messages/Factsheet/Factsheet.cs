using System.Text.Json.Serialization;

namespace VDA5050.NET.Public.Messages.Factsheet
{
    public class Factsheet : Header
    {
        [JsonPropertyName("typeSpecification")]
        public TypeSpecification? TypeSpecification { get; set; }

        [JsonPropertyName("physicalParameters")]
        public PhysicalParameters? PhysicalParameters { get; set; }

        [JsonPropertyName("protocolLimits")]
        public ProtocolLimits? ProtocolLimits { get; set; }

        [JsonPropertyName("protocolFeatures")]
        public ProtocolFeatures? ProtocolFeatures { get; set; }

        [JsonPropertyName("agvGeometry")]
        public AgvGeometry? AgvGeometry { get; set; }

        [JsonPropertyName("loadSpecification")]
        public LoadSpecification? LoadSpecification { get; set; }

        [JsonPropertyName("localizationParameters")]
        public LocalizationParameters? LocalizationParameters { get; set; }

        // you may want vendor-specific / custom data extension here
        // [JsonPropertyName("customData")]
        // public Dictionary<string, object>? CustomData { get; set; }

    }
}
