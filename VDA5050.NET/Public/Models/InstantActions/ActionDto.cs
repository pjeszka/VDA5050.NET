using VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.Order;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.State.Enums;

namespace VDA5050.NET.Public.Models.InstantActions;

public sealed record ActionDto(string Id, string ActionType, BlockingType BlockingType, List<Parameter> ActionParameters);