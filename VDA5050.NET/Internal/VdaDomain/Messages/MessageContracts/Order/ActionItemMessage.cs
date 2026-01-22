using System.Text.Json.Serialization;
using VDA5050.NET.Public.Models.InstantActions;
using Action = VDA5050.NET.Public.Models.InstantActions.Action;

namespace VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Order;

internal class ActionItemMessage
{
    [JsonPropertyName("actionId")]
    public string ActionId { get; set; } = default!;

    [JsonPropertyName("actionType")]
    public string ActionType { get; set; } = default!;

    [JsonPropertyName("blockingType")]
    public string BlockingType { get; set; } = default!;

    [JsonPropertyName("actionParameters")]
    public List<ParameterMessage> ActionParameters { get; set; } = new();

    public static ActionItemMessage CreateActionItem(Action action)
    {
        var actionItem = new ActionItemMessage()
        {
            ActionId = action.Id,
            ActionType = action.ActionType,
            BlockingType = action.BlockingType.ToString(),
            ActionParameters = action.ActionParameters
                .Select(x => ParameterMessage.Create(x.Key, x.Value))
                .ToList()
        };
        
        return actionItem;
    }
}