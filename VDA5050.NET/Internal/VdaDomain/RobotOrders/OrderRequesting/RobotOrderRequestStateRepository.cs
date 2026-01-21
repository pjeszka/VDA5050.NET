using System.Collections.Concurrent;
using VDA5050.NET.Internal.VdaDomain.RobotOrders.RobotOrderState;
using VDA5050.NET.Public.Models;

namespace VDA5050.NET.Internal.VdaDomain.RobotOrders.OrderRequesting;

public sealed class RobotOrderRequestStateRepository : IRobotOrderRequestStateRepository
{
    private readonly ConcurrentDictionary<RobotSerialNumber, CurrentRobotOrderState> _orders = new();
}