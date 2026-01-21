using VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.State;

namespace VDA5050.NET.Internal.VdaDomain.RobotOrders;

public sealed record EdgeStateDto(string Id, uint SequenceId, bool Released)
{
    public static EdgeStateDto FromMessage(EdgeState edgeState)
    {
        return new EdgeStateDto(edgeState.EdgeId, edgeState.SequenceId, edgeState.Released);
    }
}