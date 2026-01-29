using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Factsheet;

namespace VDA5050.NET.Public.Models.Robots;

public sealed record FactsheetInfo(PhysicalParametersDetails PhysicalParameters)
{
    internal static FactsheetInfo FromMessage(FactsheetMessage factsheetMessage)
    {
        return new FactsheetInfo(PhysicalParametersDetails.FromMessage(factsheetMessage.PhysicalParameters));
    }
}