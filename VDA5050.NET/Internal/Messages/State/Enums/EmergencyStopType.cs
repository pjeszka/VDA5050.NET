using System.Text.Json.Serialization;

namespace VDA5050.NET.Internal.Messages.State.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum EmergencyStopType
{
    NONE,
    AUTOACK,
    MANUAL,
    REMOTE,
    OTHER
}