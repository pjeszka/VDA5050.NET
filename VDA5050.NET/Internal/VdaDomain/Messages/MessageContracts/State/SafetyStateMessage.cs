using System.Text.Json.Serialization;
using VDA5050.NET.Public.Enums.Vda5050.State;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.State;

internal class SafetyStateMessage
{
    [JsonPropertyName("eStop")]
    public EmergencyStopType EStop { get; set; }

    [JsonPropertyName("fieldViolation")]
    public bool FieldViolation { get; set; }
}