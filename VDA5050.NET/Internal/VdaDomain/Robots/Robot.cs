using VDA5050.NET.Public.Models;

namespace VDA5050.NET.Internal.VdaDomain.Robots;

public sealed class Robot
{
    public Robot()
    {
        
    }
    
    public RobotSettings Settings { get; set; } = default!;
}