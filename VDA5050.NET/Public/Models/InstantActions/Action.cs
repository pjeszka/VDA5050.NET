using VDA5050.NET.Public.Messages.Order;
using VDA5050.NET.Public.Messages.State.Enums;

namespace VDA5050.NET.Public.Models.InstantActions;

public sealed record Action(string ActionType, BlockingType BlockingType, List<Parameter> ActionParameters);