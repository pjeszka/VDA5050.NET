using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.State;

namespace VDA5050.NET.Public.Models.Orders.OrderState;

public sealed record EdgeState(string EdgeId, uint SequenceId, bool Released, Trajectory? Trajectory)
{
    internal static EdgeState FromMessage(EdgeStateMessage edgeStateMessage)
    {
        return new EdgeState(
            edgeStateMessage.EdgeId,
            edgeStateMessage.SequenceId,
            edgeStateMessage.Released,
            edgeStateMessage.Trajectory is null ? null : Trajectory.FromMessage(edgeStateMessage.Trajectory));
    }
}