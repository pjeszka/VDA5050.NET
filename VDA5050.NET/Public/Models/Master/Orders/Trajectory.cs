using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Order;

namespace VDA5050.NET.Public.Models.Orders;

public record Trajectory(double Degree, List<ControlPoint> ControlPoints, List<double> KnotVector)
{
    internal static Trajectory FromMessage(TrajectoryMessage message)
    {
        return new Trajectory(
            message.Degree,
            message.ControlPoints.Select(ControlPoint.FromMessage).ToList(),
            message.KnotVector);
    }
}