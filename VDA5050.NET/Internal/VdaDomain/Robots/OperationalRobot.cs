using VDA5050.NET.Internal.Messages.Connection;
using VDA5050.NET.Internal.Messages.Connection.Enums;
using VDA5050.NET.Internal.Messages.Factsheet;
using VDA5050.NET.Internal.Messages.State;
using VDA5050.NET.Internal.Messages.Visualization;
using VDA5050.NET.Public.Events;
using VDA5050.NET.Public.Models;

namespace VDA5050.NET.Internal.VdaDomain.Robots;

public sealed class OperationalRobot
{
    public OperationalRobot(
        RobotSettings settings,
        EventHandler<RobotConnectionStateChangedEvent>? robotConnectionStateChangedHandler = null,
        EventHandler<RobotStateChangedEvent>? robotStateChangedHandler = null)
    {
        TopicPrefix = settings.RobotTopicPrefix;
        SerialNumber = settings.RobotSerialNumber;
        ObservedTopics = new List<string>()
        {
            $"{settings.RobotTopicPrefix}/connection",
            $"{settings.RobotTopicPrefix}/state",
            $"{settings.RobotTopicPrefix}/factsheet"
        };
        
        if (settings.ShouldObserveVisualization)
        {
            ObservedTopics.Add($"{settings.RobotTopicPrefix}/visualization");       
        }

        if (robotConnectionStateChangedHandler is not null)
        {
            RobotConnectionStateChanged += robotConnectionStateChangedHandler;
        }
        
        if (robotStateChangedHandler is not null)
        {
            RobotStateChanged += robotStateChangedHandler;
        }
    }
    
    public event EventHandler<RobotStateChangedEvent> RobotStateChanged;
    public event EventHandler<RobotConnectionStateChangedEvent> RobotConnectionStateChanged;
    
    public string TopicPrefix { get; }
    public RobotSerialNumber SerialNumber { get; }
    public ICollection<string> ObservedTopics { get; }
    public ConnectionState ConnectionState { get; private set; }
    public Factsheet? Factsheet { get; private set; }
    public State? State { get; private set; }

    public void AddStateChangeHandler(
        EventHandler<RobotStateChangedEvent> robotStateChangedHandler)
    {
        RobotStateChanged += robotStateChangedHandler;
    }
    
    public void AddConnectionStateChangeHandler(
        EventHandler<RobotConnectionStateChangedEvent> robotConnectionStateChangedHandler)
    {
        RobotConnectionStateChanged += robotConnectionStateChangedHandler;
    }

    public void OnConnectionMessage(Connection connectionMessage)
    {
        if (connectionMessage.ConnectionState != ConnectionState)
        {
            var previousConnectionState = ConnectionState;
            ConnectionState = connectionMessage.ConnectionState;
            RobotConnectionStateChanged?.Invoke(
                this,
                new RobotConnectionStateChangedEvent(SerialNumber, previousConnectionState, ConnectionState));
        }
    }
    
    public void OnStateMessage(State stateMessage)
    {
        State = stateMessage;
        RobotStateChanged?.Invoke(
            this,
            new RobotStateChangedEvent(SerialNumber, State));
    }
    
    public void OnFactsheetMessage(Factsheet factsheetMessage)
    {
        // TODO: think about refactor
        Factsheet = factsheetMessage;
    }
    
    public void OnVisualizationMessage(Visualization visualizationMessage)
    {
        throw new NotImplementedException();       
    }
    
}