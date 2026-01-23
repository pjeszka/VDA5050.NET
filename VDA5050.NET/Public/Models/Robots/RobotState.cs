using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.State;

namespace VDA5050.NET.Public.Models.Robots;

// TODO 
public sealed record RobotState(
    Pose Pose,
    RobotVelocities? Velocities)
{
    internal static RobotState FromMessage(StateMessage stateMessage)
    {
        return new RobotState(new Pose(
            stateMessage.AgvPositionMessage.X,
            stateMessage.AgvPositionMessage.Y,
            0,
            stateMessage.AgvPositionMessage.Theta),
            RobotVelocities.FromMessage(stateMessage.Velocity));
    }
}

public sealed record RobotVelocities
{
    private RobotVelocities(double vx, double vy, double omega)
    {
        Vx = vx;
        Vy = vy;
        Omega = omega;
    }

    internal static RobotVelocities? FromMessage(VelocityMessage? velocityMessage)
    {
        if (velocityMessage == null)
        {
            return null;
        }

        return new RobotVelocities(
            velocityMessage.Vx,
            velocityMessage.Vy,
            velocityMessage.Omega);
    }

    public double Vx { get; set; }
    
    public double Vy { get; set; }
    
    public double Omega { get; set; }
}