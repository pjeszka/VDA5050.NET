using System.Text.Json.Serialization;
using VDA5050.NET.Public.Enums.Vda5050.State;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.State
{
    internal class StateMessage : Header
    {
        [JsonPropertyName("orderId")]
        public string? OrderId { get; set; }

        [JsonPropertyName("orderUpdateId")]
        public uint? OrderUpdateId { get; set; }

        [JsonPropertyName("lastNodeId")]
        public string LastNodeId { get; set; } = string.Empty;

        [JsonPropertyName("lastNodeSequenceId")]
        public uint LastNodeSequenceId { get; set; }

        [JsonPropertyName("nodeStates")]
        public List<NodeStateMessage> NodeStates { get; set; }

        [JsonPropertyName("edgeStates")]
        public List<EdgeStateMessage> EdgeStates { get; set; }

        [JsonPropertyName("actionStates")]
        public List<ActionStateMessage> ActionStates { get; set; }

        [JsonPropertyName("agvPosition")]
        public AgvPositionMessage AgvPositionMessage { get; set; }

        [JsonPropertyName("velocity")]
        public VelocityMessage? Velocity { get; set; }

        [JsonPropertyName("batteryState")]
        public BatteryStateMessage? BatteryState { get; set; }

        [JsonPropertyName("operatingMode")]
        public OperatingMode OperatingMode { get; set; }

        [JsonPropertyName("driving")]
        public bool Driving { get; set; }

        [JsonPropertyName("paused")]
        public bool Paused { get; set; }

        [JsonPropertyName("safetyState")]
        public SafetyStateMessage SafetyStateMessage { get; set; }

        [JsonPropertyName("errors")]
        public List<ErrorItemMessage>? Errors { get; set; }

        //[JsonPropertyName("information")]
        //public List<InformationItem>? Information { get; set; }

        //[JsonPropertyName("distanceSinceLastNode")]
        //public double? DistanceSinceLastNode { get; set; }

        //[JsonPropertyName("loads")]
        //public List<Load>? Loads { get; set; }
    }
}
