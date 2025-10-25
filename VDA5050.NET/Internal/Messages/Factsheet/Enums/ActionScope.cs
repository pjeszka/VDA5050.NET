using System.Text.Json.Serialization;
namespace VDA5050.NET.Internal.Messages.Factsheet.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ActionScope
{
    // If you need scope of an action (e.g. node / edge)
    GLOBAL,
    NODE,
    EDGE,
    OTHER
}