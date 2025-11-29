using System.Text.Json.Serialization;
using VDA5050.NET.Public.Models.InstantActions;

namespace VDA5050.NET.Public.Messages.Order;

public class ActionItem
{
    [JsonPropertyName("actionId")]
    public string ActionId { get; set; } = default!;

    [JsonPropertyName("actionType")]
    public string ActionType { get; set; } = default!;

    [JsonPropertyName("blockingType")]
    public string BlockingType { get; set; } = default!;

    [JsonPropertyName("actionParameters")]
    public List<Parameter> ActionParameters { get; set; } = new();

    public static ActionItem CreateActionItem(ActionDto action)
    {
        var actionItem = new ActionItem()
        {
            ActionId = action.Id,
            ActionType = action.ActionType,
            BlockingType = action.BlockingType.ToString(),
            ActionParameters = action.ActionParameters
        };
        
        return actionItem;
    }
}