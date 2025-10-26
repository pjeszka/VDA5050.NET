using System.Text.Json.Serialization;

namespace VDA5050.NET.Public.Messages.Factsheet;

public class LoadSpecification
{
    [JsonPropertyName("someLoadSpecProperty")]
    public string? SomeLoadSpecProperty { get; set; }

    // define according to spec
}