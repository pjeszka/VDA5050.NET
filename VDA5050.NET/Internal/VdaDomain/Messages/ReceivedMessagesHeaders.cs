namespace VDA5050.NET.Internal.VdaDomain.Messages;

// TODO when received for the first time this should store it and then check if there was message before
public class ReceivedMessagesHeaders
{
    private uint? _stateHeaderId = null;
    private uint? _connectionHeaderId = null;
    private uint? _factsheetHeaderId = null;
    private uint? _visualizationHeaderId = null;

    /// <summary>
    /// If return false then header is from the past
    /// This can mean here that robot was restarted - and starts iterating from the beginning or received message is from the past
    /// </summary>
    /// <param name="headerId"></param>
    /// <returns></returns>
    public bool TryUpdateStateHeaderId(uint headerId)
    {
        if (_stateHeaderId.HasValue is false)
        {
            _stateHeaderId = headerId;
            return true;
        }

        if (_stateHeaderId.Value < headerId)
        {
            _stateHeaderId = headerId;
            return true;
        }

        // header id is from the past
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
        if (_connectionHeaderId.HasValue is false)
        {
            _connectionHeaderId = headerId;
            return true;
        }

        if (_connectionHeaderId.Value < headerId)
        {
            _connectionHeaderId = headerId;
            return true;
        }

        // header id is from the past
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
        if (_factsheetHeaderId.HasValue is false)
        {
            _factsheetHeaderId = headerId;
            return true;
        }

        if (_factsheetHeaderId.Value < headerId)
        {
            _factsheetHeaderId = headerId;
            return true;
        }

        // header id is from the past
        return false;
    }
}