using VDA5050.NET.Internal.Messages.State;
using VDA5050.NET.Public.Models;

namespace VDA5050.NET.Public.Events;

public sealed record RobotStateChangedEvent(RobotSerialNumber RobotSerialNumber, State State);