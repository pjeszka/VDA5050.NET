using System.Text.Json.Serialization;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.Order;

internal class ParameterMessage
{
    [JsonPropertyName("key")]
    public string Key { get; set; } = default!;

    [JsonPropertyName("value")]
    public string Value { get; set; } = default!;
    
    public static ParameterMessage Create(string key, string value) => new() { Key = key, Value = value };
}