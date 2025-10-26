using System.Text.Json.Serialization;

namespace VDA5050.NET.Public.Messages.Factsheet;

public class PhysicalParameters
{
    [JsonPropertyName("speedMin")]
    public double? SpeedMin { get; set; }

    [JsonPropertyName("speedMax")]
    public double? SpeedMax { get; set; }

    [JsonPropertyName("accelerationMax")]
    public double? AccelerationMax { get; set; }

    [JsonPropertyName("decelerationMax")]
    public double? DecelerationMax { get; set; }

    [JsonPropertyName("heightMin")]
    public double? HeightMin { get; set; }

    [JsonPropertyName("heightMax")]
    public double? HeightMax { get; set; }

    [JsonPropertyName("width")]
    public double? Width { get; set; }

    [JsonPropertyName("length")]
    public double? Length { get; set; }
}