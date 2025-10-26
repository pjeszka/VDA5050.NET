using System.Text.Json.Serialization;

namespace VDA5050.NET.Public.Messages.State.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum BlockingType
{
    NONE,
    SOFT,
    HARD,
    OTHER
}