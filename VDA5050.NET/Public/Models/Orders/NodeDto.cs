using VDA5050.NET.Public.Messages.Order;
using VDA5050.NET.Public.Models.InstantActions;

namespace VDA5050.NET.Public.Models.Orders;

public sealed record NodeDto(
    string Id,
    bool Released,
    NodePosition NodePosition,
    List<ActionDto> Actions,
    string? NodeDescription = null);