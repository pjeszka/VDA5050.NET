using System.Text.Json.Serialization;

namespace VDA5050.NET.Public.Messages.Factsheet;

public class ProtocolLimits
{
    [JsonPropertyName("idLen")]
    public uint? IdLen { get; set; }

    [JsonPropertyName("enumLen")]
    public uint? EnumLen { get; set; }

    [JsonPropertyName("maxArrayLens")]
    public MaxArrayLens? MaxArrayLens { get; set; }

    // add other limit fields if spec defines them
}