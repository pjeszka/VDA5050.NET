using VDA5050.NET.Public.Messages.Order;

namespace VDA5050.NET.Public.Models.Orders;

public sealed record NodeDto(
    string Id,
    bool Released,
    NodePosition NodePosition,
    List<Action> Actions,
    string? NodeDescription = null);