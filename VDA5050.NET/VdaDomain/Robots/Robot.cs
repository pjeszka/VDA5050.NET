using VDA5050.NET.API.Models;

namespace VDA5050.NET.VdaDomain.Robots;

public sealed class Robot
{
    public Robot()
    {
        
    }
    
    public RobotSettings Settings { get; set; } = default!;
}