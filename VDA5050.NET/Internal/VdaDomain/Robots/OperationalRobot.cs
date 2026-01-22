using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Connection;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Factsheet;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.State;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Visualization;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.Connection;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.Connection.Enums;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.Factsheet;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageModels.MessageContracts.State;
using VDA5050.NET.Public.Events;
using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Models.Robots;

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
    public FactsheetInfo? Factsheet { get; private set; }
    public RobotState? State { get; private set; }
    public Pose? Pose { get; private set; }
    
    public bool IsLocalized => Pose is not null;
    
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
            Pose = Pose.FromMessage(stateMessageMessage.AgvPositionMessage);
        }

        State = RobotState.FromMessage(stateMessageMessage);
        RobotStateChanged?.Invoke(
            this,
            new RobotStateChangedEvent(SerialNumber, State));
    }
    
    public void OnFactsheetMessage(FactsheetMessage factsheetMessageMessage)
    {
        Factsheet = FactsheetInfo.FromMessage(factsheetMessageMessage);
    }
    
    public void OnVisualizationMessage(VisualizationMessage visualizationMessageMessage)
    {
        if (_isObsevingVisualization)
        {
            UpdateRobotPosition(visualizationMessageMessage.AgvPositionMessage);
        }
    }

    private void UpdateRobotPosition(AgvPositionMessage positionMessage)
    {
        Pose = Pose.FromMessage(positionMessage);
        RobotPositionChanged?.Invoke(
            this,
            new RobotPositionChangedEvent(SerialNumber, Pose.X, Pose.Y, Pose.Theta));
    }

}