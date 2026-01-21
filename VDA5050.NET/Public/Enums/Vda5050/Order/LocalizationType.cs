using System.Text.Json.Serialization;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.Factsheet.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum LocalizationType
{
    // guessing some values; verify with spec
    NATURAL,
    REFLECTOR,
    RFID,
    DMC,
    SPOT,
    GRID,
    OTHER
}