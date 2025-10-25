using System.Text.Json.Serialization;
using VDA5050.NET.Internal.Messages.State.Enums;

namespace VDA5050.NET.Internal.Messages.State;

public class SafetyState
{
    [JsonPropertyName("eStop")]
    public EmergencyStopType EStop { get; set; }

    [JsonPropertyName("fieldViolation")]
    public bool FieldViolation { get; set; }
}