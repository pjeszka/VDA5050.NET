using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Factsheet;

namespace VDA5050.NET.Public.Models.Robots;

public sealed record PhysicalParametersDetails(double? Width, double? Length)
{
    internal static PhysicalParametersDetails FromMessage(PhysicalParametersMessage? message)
    {
        return new PhysicalParametersDetails(message?.Width ?? 1.5, message?.Length ?? 2.0);
    }
}