using System.Text.Json.Serialization;
using VDA5050.NET.Public.Models.Orders;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Order;

internal class TrajectoryMessage
{
    [JsonPropertyName("degree")]
    public double Degree { get; set; }

    [JsonPropertyName("controlPoints")]
    public List<PointMessage> ControlPoints { get; set; } = new();
    
    [JsonPropertyName("knotVector")]
    public List<double> KnotVector { get; set; } = new();

    public static TrajectoryMessage? CreateMessage(Trajectory? trajectory)
    {
        if (trajectory == null)
        {
            return null;
        }
        
        return new TrajectoryMessage()
        {
            Degree = trajectory.Degree,
            ControlPoints = trajectory.ControlPoints.Select(PointMessage.CreateMessage).ToList(),
            KnotVector = trajectory.KnotVector
        };
    }
}