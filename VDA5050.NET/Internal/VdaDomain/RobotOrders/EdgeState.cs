using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.State;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.State;

namespace VDA5050.NET.Internal.VdaDomain.RobotOrders;

public sealed record EdgeState(string Id, uint SequenceId, bool Released)
{
    internal static EdgeState FromMessage(EdgeStateMessage edgeStateMessage)
    {
        return new EdgeState(edgeStateMessage.EdgeId, edgeStateMessage.SequenceId, edgeStateMessage.Released);
    }
}