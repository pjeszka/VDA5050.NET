using VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.Order;
using VDA5050.NET.Public.Models.InstantActions;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.InstantAction;

internal class InstantActionMessage : Header
{
    public ActionItem[] Actions { get; set; }

    public static InstantActionMessage Create(
        uint headerId,
        string robotTopicPrefix,
        DateTime timeStamp,
        RobotInstantActionRequest robotInstantUpdateRequest)
    {
        var instantActionMessage = new InstantActionMessage();
        instantActionMessage.FillHeader(headerId, robotTopicPrefix, timeStamp);

        instantActionMessage.Actions =
            robotInstantUpdateRequest.Actions.Select(ActionItem.CreateActionItem).ToArray();

    }
}