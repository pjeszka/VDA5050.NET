using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.State;

namespace VDA5050.NET.Public.Models;

public sealed record Pose(double X, double Y, double Z, double Theta)
{
    internal static Pose FromMessage(AgvPositionMessage message)
    {
        return new Pose(message.X, message.Y, 0, message.Theta);
    }
}