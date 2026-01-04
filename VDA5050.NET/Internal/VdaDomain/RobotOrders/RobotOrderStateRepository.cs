using System.Collections.Concurrent;
using VDA5050.NET.Internal.VdaDomain.RobotOrders.OrderRequesting;
using VDA5050.NET.Public.Models;

namespace VDA5050.NET.Internal.VdaDomain.RobotOrders;

public sealed class RobotOrderStateRepository : IRobotOrderStateRepository
{
    private readonly ConcurrentDictionary<RobotSerialNumber, CurrentRobotOrderState> _orders = new();
}