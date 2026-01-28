using System.Text.Json.Serialization;
using VDA5050.NET.Public.Models.Orders;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Order;

internal class NodePositionMessage
{
    [JsonPropertyName("x")]
    public double X { get; set; }

    [JsonPropertyName("y")]
    public double Y { get; set; }

    [JsonPropertyName("z")]
    public double Z { get; set; }

    [JsonPropertyName("theta")]
    public double? Theta { get; set; }
    
    [JsonPropertyName("allowedDeviationXY")]
    public double? AllowedDeviationXY { get; set; }
    
    [JsonPropertyName("allowedDeviationTheta")]
    public double? AllowedDeviationTheta { get; set; }

    [JsonPropertyName("mapId")]
    public string MapId { get; set; } = default!;

    public static NodePositionMessage CreateMessage(NodePose nodeNodePose)
    {
        return new NodePositionMessage()
        {
            X = nodeNodePose.X,
            Y = nodeNodePose.Y,
            Z = nodeNodePose.Z,
            Theta = nodeNodePose.Theta,
            AllowedDeviationXY = nodeNodePose.AllowedDeviationXY,
            AllowedDeviationTheta = nodeNodePose.AllowedDeviationTheta,
            MapId = nodeNodePose.MapId
        };
    }
}
