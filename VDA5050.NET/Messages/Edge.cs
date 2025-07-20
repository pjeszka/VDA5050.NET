using System.Text.Json.Serialization;

namespace VDA5050.NET.Messages;

public class Edge
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
}
