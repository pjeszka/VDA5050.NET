using System.Text.Json.Serialization;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Factsheet;

internal class LoadSpecificatioMessage
{
    [JsonPropertyName("someLoadSpecProperty")]
    public string? SomeLoadSpecProperty { get; set; }

    // define according to spec
}