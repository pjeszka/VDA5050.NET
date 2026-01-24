using VDA5050.NET.Public.Enums.Vda5050.State;

namespace VDA5050.NET.Public.Models.InstantActions;

public sealed record Action(
    ActionId Id,
    string ActionType,
    BlockingType BlockingType,
    List<Parameter> ActionParameters);