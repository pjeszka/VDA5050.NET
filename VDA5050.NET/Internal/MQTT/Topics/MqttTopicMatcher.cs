namespace VDA5050.NET.Internal.MQTT.Topics;

internal static class MqttTopicMatcher
{
    public static bool IsMatch(string topic, string pattern)
    {
        if (pattern == "#")
            return true;

        var topicLevels = topic.Split('/', StringSplitOptions.RemoveEmptyEntries);
        var patternLevels = pattern.Split('/', StringSplitOptions.RemoveEmptyEntries);

        for (var i = 0; i < patternLevels.Length; i++)
        {
            var p = patternLevels[i];

            if (p == "#")
                return true;

            if (i >= topicLevels.Length)
                return false;

            if (p != "+" && !string.Equals(p, topicLevels[i], StringComparison.Ordinal))
                return false;
        }

        // Pattern fully matched — but make sure topic isn’t longer than pattern (if no #)
        return topicLevels.Length == patternLevels.Length;
    }
}
