using System.Text.Json.Serialization;
using VDA5050.NET.Public.Messages.Factsheet.Enums;

namespace VDA5050.NET.Public.Messages.Factsheet;

public class LocalizationParameters
{
    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("type")]
    public LocalizationType? Type { get; set; }
}