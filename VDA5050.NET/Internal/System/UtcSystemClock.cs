using VDA5050.NET.Public.Services;

namespace VDA5050.NET.Internal.System;

internal class UtcSystemClock : ISystemClock
{
    public DateTime Now => DateTime.UtcNow;
}