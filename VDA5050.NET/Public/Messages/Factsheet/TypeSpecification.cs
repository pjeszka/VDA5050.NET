using System.Text.Json.Serialization;
using VDA5050.NET.Public.Messages.Factsheet.Enums;

namespace VDA5050.NET.Public.Messages.Factsheet;

public class TypeSpecification
{
    [JsonPropertyName("seriesName")]
    public string SeriesName { get; set; } = string.Empty;

    [JsonPropertyName("seriesDescription")]
    public string? SeriesDescription { get; set; }

    [JsonPropertyName("agvKinematic")]
    public AgvKinematic AgvKinematic { get; set; }

    [JsonPropertyName("agvClass")]
    public AgvClass AgvClass { get; set; }

    [JsonPropertyName("maxLoadMass")]
    public double MaxLoadMass { get; set; }

    [JsonPropertyName("localizationTypes")]
    public List<LocalizationType>? LocalizationTypes { get; set; }

    [JsonPropertyName("navigationTypes")]
    public List<NavigationType>? NavigationTypes { get; set; }
}