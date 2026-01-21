using System.Text.Json.Serialization;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.State.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ActionStatus
{
    WAITING,
    INITIALIZING,
    RUNNING,
    PAUSED,
    FINISHED,
    FAILED,
    // possibly an UNKNOWN/OTHER fallback
    OTHER
}