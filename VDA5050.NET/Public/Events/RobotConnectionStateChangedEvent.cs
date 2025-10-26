using VDA5050.NET.Internal.Messages.Connection.Enums;
using VDA5050.NET.Public.Models;

namespace VDA5050.NET.Public.Events;

public sealed record RobotConnectionStateChangedEvent(
    RobotSerialNumber RobotSerialNumber,
    ConnectionState PreviousConnectionState,
    ConnectionState NewConnectionState);