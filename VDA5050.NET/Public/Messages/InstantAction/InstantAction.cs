using VDA5050.NET.Public.Messages.Order;

namespace VDA5050.NET.Public.Messages.InstantAction;

public class InstantAction : Header
{
    public ActionItem[] Actions { get; set; }
}