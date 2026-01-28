namespace VDA5050.NET.Internal.VdaDomain.Messages.Topics;

public static class TopicTypeMapper
{
    public static string Encode(TopicType topicType)
    {
        return topicType switch
        {
            TopicType.State => "state",
            TopicType.Visualization => "visualization",
            TopicType.Connection => "connection",
            TopicType.Factsheet => "factsheet",
            TopicType.Order => "order",
            TopicType.InstantActions => "instantActions",
            _ => throw new ArgumentOutOfRangeException(nameof(topicType), topicType, null)
        };
    }

    public static TopicType? Decode(string topic)
    {
        // get last part after / sign
        var encodedTopicType = topic.Split('/').Last();
        if (encodedTopicType == "state")
        {
            return TopicType.State;
        }

        if (encodedTopicType == "visualization")
        {
            return TopicType.Visualization;
        }

        if (encodedTopicType == "connection")
        {
            return TopicType.Connection;
        }

        if (encodedTopicType == "factsheet")
        {
            return TopicType.Factsheet;
        }
        
        if (encodedTopicType == "order")
        {
            return TopicType.Order;
        }

        if (encodedTopicType == "instantActions")
        {
            return TopicType.InstantActions;
        }

        return null;
    }
}