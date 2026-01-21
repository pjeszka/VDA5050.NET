using System.Text.Json.Serialization;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.State.Enums;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.State;

internal class SafetyState
{
    [JsonPropertyName("eStop")]
    public EmergencyStopType EStop { get; set; }

    [JsonPropertyName("fieldViolation")]
    public bool FieldViolation { get; set; }
}