using System.Text.Json.Serialization;

namespace VDA5050.NET.Public.Enums.Vda5050.Order;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum NavigationType
{
    PHYSICAL_LINE_GUIDED,
    VIRTUAL_LINE_GUIDED,
    AUTONOMOUS,
    OTHER
}