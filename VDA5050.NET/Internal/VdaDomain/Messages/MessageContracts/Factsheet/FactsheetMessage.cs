using System.Text.Json.Serialization;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Factsheet
{
    internal class FactsheetMessage : Header
    {
        [JsonPropertyName("typeSpecification")]
        public TypeSpecificationMessage? TypeSpecification { get; set; }

        [JsonPropertyName("physicalParameters")]
        public PhysicalParametersMessage? PhysicalParameters { get; set; }

        [JsonPropertyName("protocolLimits")]
        public ProtocolLimitsMessage? ProtocolLimits { get; set; }

        [JsonPropertyName("protocolFeatures")]
        public ProtocolFeaturesMessage? ProtocolFeatures { get; set; }

        [JsonPropertyName("agvGeometry")]
        public AgvGeometryMessage? AgvGeometry { get; set; }

        [JsonPropertyName("loadSpecification")]
        public LoadSpecificatioMessage? LoadSpecification { get; set; }

        [JsonPropertyName("localizationParameters")]
        public LocalizationParametersMessage? LocalizationParameters { get; set; }

        // you may want vendor-specific / custom data extension here
        // [JsonPropertyName("customData")]
        // public Dictionary<string, object>? CustomData { get; set; }

    }
}
