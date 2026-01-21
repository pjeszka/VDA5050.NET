namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageModels;

public sealed class SentMessageHeaders
{
    private uint _instantActionHeaderId = 0;
    private uint _orderHeaderId = 0;
    
    public uint GetNextInstantActionHeaderId() => Interlocked.Increment(ref _instantActionHeaderId);
    public uint GetNextOrderHeaderId() => Interlocked.Increment(ref _orderHeaderId);
}