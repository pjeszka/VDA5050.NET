using System.Text.Json.Serialization;

namespace VDA5050.NET.Public.Messages.State;

public class EdgeState
{
    [JsonPropertyName("edgeId")]
    public string EdgeId { get; set; } = string.Empty;

    [JsonPropertyName("sequenceId")]
    public uint SequenceId { get; set; }

    [JsonPropertyName("released")]
    public bool? Released { get; set; }

    // other optional fields
}