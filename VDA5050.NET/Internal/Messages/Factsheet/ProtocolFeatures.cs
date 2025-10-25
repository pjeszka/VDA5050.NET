using System.Text.Json.Serialization;

namespace VDA5050.NET.Internal.Messages.Factsheet;

public class ProtocolFeatures
{
    [JsonPropertyName("someFeatureFlag")]
    public bool? SomeFeatureFlag { get; set; }

    // Real fields depend on spec — add properties here
}