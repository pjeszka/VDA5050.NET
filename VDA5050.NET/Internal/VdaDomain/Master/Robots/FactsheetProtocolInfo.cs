using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Factsheet;

namespace VDA5050.NET.Internal.VdaDomain.Robots;

internal sealed record FactsheetProtocolInfo
{
    private FactsheetProtocolInfo(ProtocolLimits? protocolLimits)
    {
        ProtocolLimits = protocolLimits;
    }

    internal static FactsheetProtocolInfo FromMessage(FactsheetMessage factsheetMessage)
    {
        return new FactsheetProtocolInfo(ProtocolLimits.FromMessage(factsheetMessage.ProtocolLimits));
    }
    
    public ProtocolLimits? ProtocolLimits { get; }
}

internal sealed record MaxArrayLens
{
    private MaxArrayLens(uint? orderNodes, uint? orderEdges, uint? nodeActions, uint? edgeActions)
    {
        OrderNodes = orderNodes;
        OrderEdges = orderEdges;
        NodeActions = nodeActions;
        EdgeActions = edgeActions;
    }

    public static MaxArrayLens? FromMessage(MaxArrayLensMessage? maxArrayLensMessage)
    {
        if (maxArrayLensMessage == null)
        {
            return null;
        }

        return new MaxArrayLens(
            maxArrayLensMessage.OrderNodes,
            maxArrayLensMessage.OrderEdges,
            maxArrayLensMessage.NodeActions,
            maxArrayLensMessage.EdgeActions);
    }
    
    public uint? OrderNodes { get; private set; }
    
    public uint? OrderEdges { get; private set; }
    
    public uint? NodeActions { get; private set; }
    
    public uint? EdgeActions { get; private set; }
}

internal sealed record ProtocolLimits
{
    private ProtocolLimits(uint? idLen, uint? enumLen, MaxArrayLens? maxArrayLens)
    {
        IdLen = idLen;
        EnumLen = enumLen;
        MaxArrayLens = maxArrayLens;
    }
    
    public static ProtocolLimits? FromMessage(ProtocolLimitsMessage? protocolLimitsMessage)
    {
        if (protocolLimitsMessage == null)
        {
            return null;
        }
        
        return new ProtocolLimits(
            protocolLimitsMessage.IdLen,
            protocolLimitsMessage.EnumLen,
            MaxArrayLens.FromMessage(protocolLimitsMessage.MaxArrayLens));
    }
    
    public uint? IdLen { get; set; }
    
    public uint? EnumLen { get; set; }
    
    public MaxArrayLens? MaxArrayLens { get; set; }
}