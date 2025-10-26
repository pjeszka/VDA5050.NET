using System.Text.Json.Serialization;
using VDA5050.NET.Public.Messages.State.Enums;

namespace VDA5050.NET.Public.Messages.State;

public class SafetyState
{
    [JsonPropertyName("eStop")]
    public EmergencyStopType EStop { get; set; }

    [JsonPropertyName("fieldViolation")]
    public bool FieldViolation { get; set; }
}