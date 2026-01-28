namespace VDA5050.NET.Internal.VdaDomain.Messages;

// TODO when received for the first time this should store it and then check if there was message before
internal class ReceivedMessagesHeaders
{
    public uint? StateHeaderId { get; private set; } = null;
    public uint? ConnectionHeaderId { get; private set; } = null;
    public uint? FactsheetHeaderId { get; private set; } = null;
    public uint? VisualizationHeaderId { get; private set; } = null;

    /// <summary>
    /// If return false then header is from the past
    /// This can mean here that robot was restarted - and starts iterating from the beginning or received message is from the past
    /// </summary>
    /// <param name="headerId"></param>
    /// <returns></returns>
    public bool TryUpdateStateHeaderId(uint headerId)
    {
        if (StateHeaderId.HasValue is false)
        {
            StateHeaderId = headerId;
            return true;
        }

        if (StateHeaderId.Value < headerId)
        {
            StateHeaderId = headerId;
            return true;
        }

        // header id is from the past
        if (headerId == uint.MinValue || StateHeaderId.Value == uint.MaxValue)
        {
            StateHeaderId = null;
        }
        
        return false;
    }
    
    /// <summary>
    /// If return false then header is from the past
    /// This can mean here that robot was restarted - and starts iterating from the beginning
    /// </summary>
    /// <param name="headerId"></param>
    /// <returns></returns>
    public bool TryUpdateConnectionHeaderId(uint headerId)
    {
        if (ConnectionHeaderId.HasValue is false)
        {
            ConnectionHeaderId = headerId;
            return true;
        }

        if (ConnectionHeaderId.Value < headerId)
        {
            ConnectionHeaderId = headerId;
            return true;
        }

        // header id is from the past
        if (headerId == uint.MinValue || ConnectionHeaderId.Value == uint.MaxValue)
        {
            ConnectionHeaderId = null;
        }
        
        return false;
    }
    
    
    /// <summary>
    /// If return false then header is from the past
    /// This can mean here that robot was restarted - and starts iterating from the beginning
    /// </summary>
    /// <param name="headerId"></param>
    /// <returns></returns>
    public bool TryUpdateFactsheetHeaderId(uint headerId)
    {
        if (FactsheetHeaderId.HasValue is false)
        {
            FactsheetHeaderId = headerId;
            return true;
        }

        if (FactsheetHeaderId.Value < headerId)
        {
            FactsheetHeaderId = headerId;
            return true;
        }

        // header id is from the past
        if (headerId == uint.MinValue || FactsheetHeaderId.Value == uint.MaxValue)
        {
            FactsheetHeaderId = null;
        }

        return false;
    }
    
    /// <summary>
    /// If return false then header is from the past
    /// This can mean here that robot was restarted - and starts iterating from the beginning
    /// </summary>
    /// <param name="headerId"></param>
    /// <returns></returns>
    public bool TryUpdateVisualizationHeaderId(uint headerId)
    {
        if (VisualizationHeaderId.HasValue is false)
        {
            VisualizationHeaderId = headerId;
            return true;
        }

        if (VisualizationHeaderId.Value < headerId)
        {
            VisualizationHeaderId = headerId;
            return true;
        }

        // header id is from the past
        if (headerId == uint.MinValue || VisualizationHeaderId.Value == uint.MaxValue)
        {
            VisualizationHeaderId = null;
        }
        
        return false;
    }
}