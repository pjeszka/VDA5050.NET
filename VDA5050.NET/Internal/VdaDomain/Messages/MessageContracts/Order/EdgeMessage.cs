using System.Text.Json.Serialization;
using VDA5050.NET.Public.Models.Orders;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Order;

internal class EdgeMessage
{
    [JsonPropertyName("edgeId")]
    public string EdgeId { get; set; } = default!;

    [JsonPropertyName("sequenceId")]
    public uint SequenceId { get; set; }

    [JsonPropertyName("edgeDescription")]
    public string? EdgeDescription { get; set; }

    [JsonPropertyName("released")]
    public bool Released { get; set; }

    [JsonPropertyName("startNodeId")]
    public string StartNodeId { get; set; } = default!;

    [JsonPropertyName("endNodeId")]
    public string EndNodeId { get; set; } = default!;

    [JsonPropertyName("maxSpeed")]
    public double? MaxSpeed { get; set; }

    [JsonPropertyName("maxHeight")]
    public double? MaxHeight { get; set; }

    [JsonPropertyName("minHeight")]
    public double? MinHeight { get; set; }

    [JsonPropertyName("orientation")]
    public double? Orientation { get; set; }

    [JsonPropertyName("orientationType")]
    public string? OrientationType { get; set; }

    [JsonPropertyName("direction")]
    public string? Direction { get; set; }

    [JsonPropertyName("rotationAllowed")]
    public bool? RotationAllowed { get; set; }

    [JsonPropertyName("maxRotationSpeed")]
    public double? MaxRotationSpeed { get; set; }

    [JsonPropertyName("length")]
    public double? Length { get; set; }

    [JsonPropertyName("trajectory")]
    public TrajectoryMessage? Trajectory { get; set; }

    [JsonPropertyName("actions")]
    public List<ActionItemMessage> Actions { get; set; } = new();
    
    public static EdgeMessage CreateEdgeMessage(Edge edge, uint index)
    {
        return new EdgeMessage { 
            EdgeId = edge.Id, 
            SequenceId = index,
            StartNodeId = edge.StartNodeId,
            EndNodeId = edge.EndNodeId,
            Released = edge.Released,
            EdgeDescription = edge.EdgeDescription,
            MaxSpeed = edge.EdgeParams?.SpeedParams?.MaxSpeed,
            MaxRotationSpeed = edge.EdgeParams?.SpeedParams?.MaxRotationSpeep,
            MaxHeight = edge.EdgeParams?.DimensionParams?.MaxHeight,
            MinHeight = edge.EdgeParams?.DimensionParams?.MinHeight,
            Orientation = edge.OrientationParams?.Orientation,
            OrientationType = edge.OrientationParams?.OrientationType.ToString(),
            Direction = edge.Direction,
            RotationAllowed = edge.OrientationParams?.RotationAllowed,
            Length = edge.Length,
            Trajectory = TrajectoryMessage.CreateMessage(edge.Trajectory),
            Actions = edge.Actions.Select(ActionItemMessage.CreateActionItem).ToList()
        };
    }
}
