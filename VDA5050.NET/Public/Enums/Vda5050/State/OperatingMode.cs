using System.Text.Json.Serialization;

namespace VDA5050.NET.Public.Enums.Vda5050.State;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OperatingMode
{
    AUTOMATIC,
    SEMIAUTOMATIC,
    MANUAL,
    SERVICE,
    TEACHIN,
    OTHER
}