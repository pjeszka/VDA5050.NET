using System.Text.Json.Serialization;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.Factsheet;

internal class LoadSpecification
{
    [JsonPropertyName("someLoadSpecProperty")]
    public string? SomeLoadSpecProperty { get; set; }

    // define according to spec
}