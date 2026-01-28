using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Order;

namespace VDA5050.NET.Public.Models.Orders;

public sealed record ControlPoint(double X, double Y, double? Weight)
{
    internal static ControlPoint FromMessage(PointMessage message)
    {
        return new ControlPoint(
            message.X,
            message.Y,
            message.Weight
        );
    }
}