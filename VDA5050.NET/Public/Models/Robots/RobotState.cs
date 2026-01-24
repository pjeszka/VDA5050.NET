using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.State;
using VDA5050.NET.Public.Enums.Vda5050.State;

namespace VDA5050.NET.Public.Models.Robots;

public sealed record RobotState(
    Pose? Pose,
    RobotVelocities? Velocities,
    OperatingMode OperatingMode,
    BatteryState? BatteryState,
    bool IsDriving,
    bool? IsPaused,
    SafetyState SafetyState)
{
    internal static RobotState FromMessage(StateMessage stateMessage)
    {
        return new RobotState(Pose.FromMessage(stateMessage.AgvPosition),
            RobotVelocities.FromMessage(stateMessage.Velocity),
            stateMessage.OperatingMode,
            BatteryState.FromMessage(stateMessage.BatteryState),
            stateMessage.Driving,
            stateMessage.Paused,
            SafetyState.FromMessage(stateMessage.SafetyState));
    }
}

public sealed record SafetyState
{
    internal static SafetyState FromMessage(SafetyStateMessage? message)
    {
        if (message == null)
        {
            return null;
        }
        
        return new SafetyState();
    }
}

public sealed record BatteryState
{
    private BatteryState(double batteryCharge, bool charging, double? batteryVoltage, int? batteryHealth, uint? reach)
    {
    }

    internal static BatteryState? FromMessage(BatteryStateMessage? message)
    {
        if (message == null)
        {
            return null;
        }

        return new BatteryState(
            message.BatteryCharge, 
            message.Charging,
            message.BatteryVoltage,
            message.BatteryHealth,
            message.Reach);
    }
    
    public double BatteryCharge { get; set; }
    public bool Charging { get; set; }
    public double? BatteryVoltage { get; set; }
    public int? BatteryHealth { get; set; }
    public uint? Reach { get; set; }
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