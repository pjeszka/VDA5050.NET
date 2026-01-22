using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Order;
using VDA5050.NET.Public.Models.InstantActions;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.InstantAction;

internal class InstantActionMessage : Header
{
    public ActionItemMessage[] Actions { get; set; }

    public static InstantActionMessage Create(
        uint headerId,
        string robotTopicPrefix,
        DateTime timeStamp,
        RobotInstantActionRequest robotInstantUpdateRequest)
    {
        var instantActionMessage = new InstantActionMessage();
        instantActionMessage.FillHeader(headerId, robotTopicPrefix, timeStamp);

        instantActionMessage.Actions =
            robotInstantUpdateRequest.Actions.Select(ActionItemMessage.CreateActionItem).ToArray();

        return instantActionMessage;
    }
}