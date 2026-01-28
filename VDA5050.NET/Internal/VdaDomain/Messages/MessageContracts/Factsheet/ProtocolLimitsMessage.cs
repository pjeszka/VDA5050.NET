using System.Text.Json.Serialization;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Factsheet;

internal class ProtocolLimitsMessage
{
    [JsonPropertyName("idLen")]
    public uint? IdLen { get; set; }

    [JsonPropertyName("enumLen")]
    public uint? EnumLen { get; set; }

    [JsonPropertyName("maxArrayLens")]
    public MaxArrayLensMessage? MaxArrayLens { get; set; }

    // add other limit fields if spec defines them
}