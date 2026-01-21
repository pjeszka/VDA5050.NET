using System.Text.Json.Serialization;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.Factsheet.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum WheelType
{
    DRIVE,
    CASTER,
    FIXED,
    MECANUM,
    OTHER
}