using System.Text.Json.Serialization;

namespace VDA5050.NET.Messages;

public class Parameter
{
    [JsonPropertyName("key")]
    public string Key { get; set; } = default!;

    [JsonPropertyName("value")]
    public string Value { get; set; } = default!;
}