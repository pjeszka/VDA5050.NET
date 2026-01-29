using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Order;

namespace VDA5050.NET.Public.Models.Orders;

public sealed record NodePose(
    double X,
    double Y,
    double Z,
    double? Theta,
    string MapId,
    double? AllowedDeviationXY,
    double? AllowedDeviationTheta)
{
    internal static NodePose? FromMessage(NodePositionMessage? message)
    {
        if (message is null)
        {
            return null;
        }

        return new NodePose(
            message.X,
            message.Y,
            message.Z,
            message.Theta,
            message.MapId,
            message.AllowedDeviationXY,
            message.AllowedDeviationTheta);
    }
}