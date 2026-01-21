using System.Text.Json.Serialization;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.State.Enums;

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