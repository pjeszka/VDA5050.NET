using System.Text.Json.Serialization;
using VDA5050.NET.Public.Models.Orders;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.Order;

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
    public Trajectory? Trajectory { get; set; }

    [JsonPropertyName("actions")]
    public List<ActionItem> Actions { get; set; } = new();
    
    public static EdgeMessage CreateEdgeMessage(EdgeDto edgeDto, uint index)
    {
        return new EdgeMessage { 
            EdgeId = edgeDto.Id, 
            SequenceId = index,
            StartNodeId = edgeDto.StartNodeId,
            EndNodeId = edgeDto.EndNodeId,
            Released = edgeDto.Released,
            EdgeDescription = edgeDto.EdgeDescription,
            MaxSpeed = edgeDto.EdgeParams?.SpeedParams?.MaxSpeed,
            MaxRotationSpeed = edgeDto.EdgeParams?.SpeedParams?.MaxRotationSpeep,
            MaxHeight = edgeDto.EdgeParams?.DimensionParams?.MaxHeight,
            MinHeight = edgeDto.EdgeParams?.DimensionParams?.MinHeight,
            Orientation = edgeDto.OrientationParams?.Orientation,
            OrientationType = edgeDto.OrientationParams?.OrientationType.ToString(),
            Direction = edgeDto.Direction,
            RotationAllowed = edgeDto.OrientationParams?.RotationAllowed,
            Length = edgeDto.Length,
            Trajectory = edgeDto.Trajectory,
            Actions = edgeDto.Actions.Select(ActionItem.CreateActionItem).ToList()
        };
    }
}
