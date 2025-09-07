namespace VDA5050.NET.MQTT.Topics;

public enum TopicType
{
    // From robot
    State = 0,
    Connection = 1,
    Factsheet = 2,
    Visualization = 3,
    // From master
    InstantActions = 4,
    Order = 5,
    
}