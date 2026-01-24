using System.Text.Json.Serialization;
using VDA5050.NET.Public.Enums.Vda5050.State;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.State
{
internal sealed class StateMessage : Header
{
    // ----------------------------
    // Order-related
    // ----------------------------

    [JsonPropertyName("orderId")]
    public string? OrderId { get; set; }

    [JsonPropertyName("orderUpdateId")]
    public uint? OrderUpdateId { get; set; }

    [JsonPropertyName("lastNodeId")]
    public string LastNodeId { get; set; } = string.Empty;

    [JsonPropertyName("lastNodeSequenceId")]
    public uint LastNodeSequenceId { get; set; }

    [JsonPropertyName("distanceSinceLastNode")]
    public double? DistanceSinceLastNode { get; set; }

    [JsonPropertyName("newBaseRequest")]
    public bool? NewBaseRequest { get; set; }

    // ----------------------------
    // Graph state
    // ----------------------------

    [JsonPropertyName("nodeStates")]
    public List<NodeStateMessage> NodeStates { get; set; } = new();

    [JsonPropertyName("edgeStates")]
    public List<EdgeStateMessage> EdgeStates { get; set; } = new();

    // ----------------------------
    // Actions
    // ----------------------------

    [JsonPropertyName("actionStates")]
    public List<ActionStateMessage> ActionStates { get; set; } = new();

    // ----------------------------
    // Position & motion
    // ----------------------------

    [JsonPropertyName("agvPosition")]
    public AgvPositionMessage? AgvPosition { get; set; }

    [JsonPropertyName("velocity")]
    public VelocityMessage? Velocity { get; set; }

    // ----------------------------
    // Energy
    // ----------------------------

    [JsonPropertyName("batteryState")]
    public BatteryStateMessage? BatteryState { get; set; }

    // ----------------------------
    // Operating state
    // ----------------------------

    [JsonPropertyName("operatingMode")]
    public OperatingMode OperatingMode { get; set; }

    [JsonPropertyName("driving")]
    public bool Driving { get; set; }

    [JsonPropertyName("paused")]
    public bool? Paused { get; set; }

    // ----------------------------
    // Safety
    // ----------------------------

    [JsonPropertyName("safetyState")]
    public SafetyStateMessage SafetyState { get; set; } = default!;

    // ----------------------------
    // Diagnostics
    // ----------------------------

    [JsonPropertyName("errors")]
    public List<ErrorItemMessage>? Errors { get; set; }

    [JsonPropertyName("information")]
    public List<InformationItemMessage>? Information { get; set; }

    // ----------------------------
    // Load handling
    // ----------------------------

    [JsonPropertyName("loads")]
    public List<LoadMessage>? Loads { get; set; }
}
}
