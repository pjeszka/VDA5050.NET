using System.Text.Json.Serialization;
using VDA5050.NET.Public.Messages.State.Enums;

namespace VDA5050.NET.Public.Messages.State
{
    public class State : Header
    {
        [JsonPropertyName("orderId")]
        public string? OrderId { get; set; }

        [JsonPropertyName("orderUpdateId")]
        public uint? OrderUpdateId { get; set; }

        [JsonPropertyName("lastNodeId")]
        public string? LastNodeId { get; set; }

        [JsonPropertyName("lastNodeSequenceId")]
        public uint? LastNodeSequenceId { get; set; }

        [JsonPropertyName("nodeStates")]
        public List<NodeState>? NodeStates { get; set; }

        [JsonPropertyName("edgeStates")]
        public List<EdgeState>? EdgeStates { get; set; }

        [JsonPropertyName("actionStates")]
        public List<ActionState>? ActionStates { get; set; }

        [JsonPropertyName("agvPosition")]
        public AgvPosition AgvPosition { get; set; }

        [JsonPropertyName("velocity")]
        public Velocity? Velocity { get; set; }

        [JsonPropertyName("batteryState")]
        public BatteryState? BatteryState { get; set; }

        [JsonPropertyName("operatingMode")]
        public OperatingMode OperatingMode { get; set; }

        [JsonPropertyName("driving")]
        public bool Driving { get; set; }

        [JsonPropertyName("paused")]
        public bool Paused { get; set; }

        [JsonPropertyName("safetyState")]
        public SafetyState SafetyState { get; set; }

        [JsonPropertyName("errors")]
        public List<ErrorItem>? Errors { get; set; }

        //[JsonPropertyName("information")]
        //public List<InformationItem>? Information { get; set; }

        //[JsonPropertyName("distanceSinceLastNode")]
        //public double? DistanceSinceLastNode { get; set; }

        //[JsonPropertyName("loads")]
        //public List<Load>? Loads { get; set; }
    }
}
