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
    bool? NewBaseRequest,
    SafetyState SafetyState,
    ICollection<Load>? Loads)
{
    internal static RobotState FromMessage(StateMessage stateMessage)
    {
        return new RobotState(Pose.FromMessage(stateMessage.AgvPosition),
            RobotVelocities.FromMessage(stateMessage.Velocity),
            stateMessage.OperatingMode,
            BatteryState.FromMessage(stateMessage.BatteryState),
            stateMessage.Driving,
            stateMessage.Paused,
            stateMessage.NewBaseRequest,
            SafetyState.FromMessage(stateMessage.SafetyState),
            stateMessage.Loads is null ? null : stateMessage.Loads.Select(Load.FromMessage).ToList());
    }
}

public sealed record BoundingBoxReference
{
    private BoundingBoxReference(double x, double y, double z, double? theta)
    {
        X = x;
        Y = y;
        Z = z;
        Theta = theta;
    }

    internal static BoundingBoxReference? FromMessage(BoundingBoxReferenceMessage? message)
    {
        if (message == null)
        {
            return null;
        }
        
        return new BoundingBoxReference(message.X, message.Y, message.Z, message.Theta);
    }

    public double X { get; }
    public double Y { get; }
    public double Z { get; }
    public double? Theta { get; }
}

public sealed record LoadDimensions
{
    private LoadDimensions(double length, double width, double? height)
    {
        Length = length;
        Width = width;
        Height = height;
    }
    
    internal static LoadDimensions? FromMessage(LoadDimensionsMessage? message)
    {
        if (message == null)
        {
            return null;
        }

        return new LoadDimensions(message.Length, message.Width, message.Height);
    } 
    public double Length { get; }
    public double Width { get; }
    public double? Height { get; }
}

public sealed record Load
{
    private Load(
        string? loadId,
        string? loadType,
        string? loadPosition,
        double? weight,
        BoundingBoxReference? boundingBoxReference,
        LoadDimensions? dimensions)
    {
        LoadId = loadId;
        LoadType = loadType;
        LoadPosition = loadPosition;
        Weight = weight;
        BoundingBoxReference = boundingBoxReference;
        Dimensions = dimensions;
    }

    internal static Load FromMessage(LoadMessage message)
    {
        return new Load(
            message.LoadId,
            message.LoadType,
            message.LoadPosition,
            message.Weight,
            BoundingBoxReference.FromMessage(message.BoundingBoxReference),
            LoadDimensions.FromMessage(message.Dimensions)
        );
    }

    public string? LoadId { get; } = string.Empty;
    public string? LoadType { get; }
    public string? LoadPosition { get; }
    public double? Weight { get; }
    public BoundingBoxReference? BoundingBoxReference { get; }
    public LoadDimensions? Dimensions { get; }
}

public sealed record SafetyState
{
    private SafetyState(EmergencyStopType eStop, bool fieldViolation)
    {
        EStop = eStop;
        FieldViolation = fieldViolation;
    }

    internal static SafetyState FromMessage(SafetyStateMessage? message)
    {
        if (message == null)
        {
            return null;
        }
        
        return new SafetyState(message.EStop, message.FieldViolation);
    }
    
    public EmergencyStopType EStop { get; }
    
    public bool FieldViolation { get; }
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
    
    public double BatteryCharge { get; }
    public bool Charging { get; }
    public double? BatteryVoltage { get; }
    public int? BatteryHealth { get; }
    public uint? Reach { get; }
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

    public double Vx { get; }
    
    public double Vy { get; }
    
    public double Omega { get; }
}