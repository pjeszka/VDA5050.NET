using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.InstantAction;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Order;

namespace VDA5050.NET.Internal.VdaDomain.Robots;

internal static class RobotMessageValidator
{
    public static void ValidateOrderMessage(OrderMessage message, FactsheetProtocolInfo? protocolInfo)
    {
        if (protocolInfo == null)
        {
            return;
        }
        
        var limits = protocolInfo.ProtocolLimits?.MaxArrayLens;
        var idLen = protocolInfo.ProtocolLimits?.IdLen;
        
        if (limits != null)
        {
            if (limits.OrderNodes.HasValue && message.Nodes.Count > limits.OrderNodes.Value)
                throw new InvalidOperationException(
                    $"Too many nodes: {message.Nodes.Count}, max allowed: {limits.OrderNodes.Value}");

            if (limits.OrderEdges.HasValue && message.Edges.Count > limits.OrderEdges.Value)
                throw new InvalidOperationException(
                    $"Too many edges: {message.Edges.Count}, max allowed: {limits.OrderEdges.Value}");

            if (limits.NodeActions.HasValue && message.Nodes.Any(n => n.Actions.Count > limits.NodeActions.Value))
                throw new InvalidOperationException(
                    $"A node has too many actions, max allowed: {limits.NodeActions.Value}");

            if (limits.EdgeActions.HasValue && message.Edges.Any(e => e.Actions.Count > limits.EdgeActions.Value))
                throw new InvalidOperationException(
                    $"An edge has too many actions, max allowed: {limits.EdgeActions.Value}");
        }
        
        if (idLen.HasValue)
        {
            if (message.OrderId.Length > idLen.Value)
            {
                throw new InvalidOperationException(
                    $"Order ID '{message.OrderId}' exceeds max length: {idLen.Value}");
            }

            foreach (var node in message.Nodes)
            {
                if (node.NodeId.Length > idLen.Value)
                    throw new InvalidOperationException(
                        $"Node ID '{node.NodeId}' exceeds max length: {idLen.Value}");
                
                foreach (var action in node.Actions)
                {
                    if (action.ActionId.Length > idLen.Value)
                        throw new InvalidOperationException(
                            $"Node action ID '{action.ActionId}' exceeds max length: {idLen.Value}");
                }
            }

            foreach (var edge in message.Edges)
            {
                if (edge.EdgeId.Length > idLen.Value)
                    throw new InvalidOperationException(
                        $"Edge ID '{edge.EdgeId}' exceeds max length: {idLen.Value}");

                foreach (var action in edge.Actions)
                {
                    if (action.ActionId.Length > idLen.Value)
                        throw new InvalidOperationException(
                            $"Edge action ID '{action.ActionId}' exceeds max length: {idLen.Value}");
                }
            }
        }
    }

    public static void ValidateInstantActionMessage(InstantActionMessage message, FactsheetProtocolInfo? protocolInfo)
    {
        if (protocolInfo == null)
        {
            return;
        }
        
        var idLen = protocolInfo.ProtocolLimits?.IdLen;
        var maxActions = protocolInfo.ProtocolLimits?.MaxArrayLens?.NodeActions;
        
        if (idLen.HasValue)
        {
            foreach (var action in message.Actions)
            {
                if (action.ActionId.Length > idLen.Value)
                    throw new InvalidOperationException(
                        $"Instant action ID '{action.ActionId}' exceeds max length: {idLen.Value}");
            }
        }
        
        if (maxActions.HasValue && message.Actions.Length > maxActions.Value)
        {
            throw new InvalidOperationException(
                $"Too many instant actions: {message.Actions.Length}, max allowed: {maxActions.Value}");
        }
    }
}