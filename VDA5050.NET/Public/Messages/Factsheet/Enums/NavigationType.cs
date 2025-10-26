using System.Text.Json.Serialization;

namespace VDA5050.NET.Public.Messages.Factsheet.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum NavigationType
{
    PHYSICAL_LINE_GUIDED,
    VIRTUAL_LINE_GUIDED,
    AUTONOMOUS,
    OTHER
}