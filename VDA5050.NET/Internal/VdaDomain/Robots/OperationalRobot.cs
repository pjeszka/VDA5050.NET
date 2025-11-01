using VDA5050.NET.Public.Events;
using VDA5050.NET.Public.Messages.Connection;
using VDA5050.NET.Public.Messages.Connection.Enums;
using VDA5050.NET.Public.Messages.Factsheet;
using VDA5050.NET.Public.Messages.State;
using VDA5050.NET.Public.Messages.Visualization;
using VDA5050.NET.Public.Models;

namespace VDA5050.NET.Internal.VdaDomain.Robots;

public sealed class OperationalRobot
{
    private readonly bool _isObsevingVisualization;
    public OperationalRobot(
        RobotSettings settings)
    {
        TopicPrefix = settings.RobotTopicPrefix;
        SerialNumber = settings.RobotSerialNumber;
        ObservedTopics = new List<string>()
        {
            $"{settings.RobotTopicPrefix}/connection",
            $"{settings.RobotTopicPrefix}/state",
            $"{settings.RobotTopicPrefix}/factsheet"
        };

        _isObsevingVisualization = settings.ShouldObserveVisualization;
        if (settings.ShouldObserveVisualization)
        {
            ObservedTopics.Add($"{settings.RobotTopicPrefix}/visualization");       
        }
    }
    
    private event EventHandler<RobotPositionChangedEvent>? RobotPositionChanged;
    private event EventHandler<RobotStateChangedEvent>? RobotStateChanged;
    private event EventHandler<RobotConnectionStateChangedEvent>? RobotConnectionStateChanged;
    
    public string TopicPrefix { get; }
    public RobotSerialNumber SerialNumber { get; }
    public ICollection<string> ObservedTopics { get; }
    public ConnectionState ConnectionState { get; private set; }
    public Factsheet? Factsheet { get; private set; }
    public State? State { get; private set; }

    public AgvPosition? Position { get; private set; }
    
    public bool IsLocalized => Position is not null;
    
    public void AddPositionChangeHandler(
        EventHandler<RobotPositionChangedEvent> robotPositionChangedHandler)
    {
        RobotPositionChanged += robotPositionChangedHandler;
    }

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
        if (_isObsevingVisualization is false)
        {
            Position = stateMessage.AgvPosition;
        }

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
        if (_isObsevingVisualization)
        {
            UpdateRobotPosition(visualizationMessage.AgvPosition);
        }
    }

    private void UpdateRobotPosition(AgvPosition position)
    {
        Position = position;
        RobotPositionChanged?.Invoke(
            this,
            new RobotPositionChangedEvent(SerialNumber, Position.X, Position.Y, Position.Theta));
    }

}