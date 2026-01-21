using VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.Connection;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.Connection.Enums;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.Factsheet;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.State;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.Visualization;
using VDA5050.NET.Public.Events;
using VDA5050.NET.Public.Models;

namespace VDA5050.NET.Internal.VdaDomain.Robots;

internal sealed class OperationalRobot
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
    public FactsheetMessage? Factsheet { get; private set; }
    public StateMessage? State { get; private set; }

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

    internal void OnConnectionMessage(ConnectionMessage connectionMessageMessage)
    {
        if (connectionMessageMessage.ConnectionState != ConnectionState)
        {
            var previousConnectionState = ConnectionState;
            ConnectionState = connectionMessageMessage.ConnectionState;
            RobotConnectionStateChanged?.Invoke(
                this,
                new RobotConnectionStateChangedEvent(SerialNumber, previousConnectionState, ConnectionState));
        }
    }
    
    public void OnStateMessage(StateMessage stateMessageMessage)
    {
        if (_isObsevingVisualization is false)
        {
            Position = stateMessageMessage.AgvPosition;
        }

        State = stateMessageMessage;
        RobotStateChanged?.Invoke(
            this,
            new RobotStateChangedEvent(SerialNumber, State));
    }
    
    public void OnFactsheetMessage(FactsheetMessage factsheetMessageMessage)
    {
        // TODO: think about refactor
        Factsheet = factsheetMessageMessage;
    }
    
    public void OnVisualizationMessage(VisualizationMessage visualizationMessageMessage)
    {
        if (_isObsevingVisualization)
        {
            UpdateRobotPosition(visualizationMessageMessage.AgvPosition);
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