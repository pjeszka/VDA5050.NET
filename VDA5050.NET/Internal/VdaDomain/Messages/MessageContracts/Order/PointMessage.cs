using System.Text.Json.Serialization;
using VDA5050.NET.Public.Models.Orders;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Order;

internal class PointMessage
{
    [JsonPropertyName("x")]
    public double X { get; set; }

    [JsonPropertyName("y")]
    public double Y { get; set; }

    [JsonPropertyName("weight")]
    public double? Weight { get; set; }

    public static PointMessage CreateMessage(ControlPoint arg)
    {
        return new PointMessage()
        {
            X = arg.X,
            Y = arg.Y,
            Weight = arg.Weight
        };
    }
}