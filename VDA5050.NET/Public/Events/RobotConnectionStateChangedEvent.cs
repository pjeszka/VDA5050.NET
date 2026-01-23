using VDA5050.NET.Public.Enums.Vda5050.Connection;
using VDA5050.NET.Public.Models;

namespace VDA5050.NET.Public.Events;

public sealed record RobotConnectionStateChangedEvent(
    RobotSerialNumber RobotSerialNumber,
    ConnectionState PreviousConnectionState,
    ConnectionState NewConnectionState);