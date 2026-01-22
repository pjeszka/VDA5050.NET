using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Order;
using VDA5050.NET.Public.Models.InstantActions;
using Action = VDA5050.NET.Public.Models.InstantActions.Action;

namespace VDA5050.NET.Public.Models.Orders;

public sealed record Edge(
    string Id,
    bool Released,
    string StartNodeId,
    string EndNodeId,
    List<Action> Actions,
    OrientationParams? OrientationParams = null,
    Trajectory? Trajectory = null,
    double? Length = null,
    string? Direction = null,
    EdgeParams? EdgeParams = null,
    string? EdgeDescription = null);

public enum OrientationType {
    GLOBAL,
    TANGENTIAL
}

public record OrientationParams(
    double Orientation,
    OrientationType OrientationType = OrientationType.TANGENTIAL,
    bool RotationAllowed = true);
    
public record EdgeParams(
    SpeedParams? SpeedParams = null,
    DimensionParams? DimensionParams = null);

public record SpeedParams(
    double? MaxSpeed = null,
    double? MaxRotationSpeep = null);

public record DimensionParams(
    double? MaxHeight = null,
    double? MinHeight = null);