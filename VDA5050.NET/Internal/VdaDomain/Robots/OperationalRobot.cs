using Microsoft.Extensions.Logging;
using VDA5050.NET.Internal.VdaDomain.Messages;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Connection;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Factsheet;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.State;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Visualization;
using VDA5050.NET.Internal.VdaDomain.RobotOrders;
using VDA5050.NET.Public.Enums.Domain;
using VDA5050.NET.Public.Enums.Vda5050.Connection;
using VDA5050.NET.Public.Events;
using VDA5050.NET.Public.Models;
using VDA5050.NET.Public.Models.Errors;
using VDA5050.NET.Public.Models.Orders;
using VDA5050.NET.Public.Models.Orders.OrderState;
using VDA5050.NET.Public.Models.Robots;

namespace VDA5050.NET.Internal.VdaDomain.Robots;

internal sealed class OperationalRobot
{
    private readonly bool _isObsevingVisualization;
    private readonly ReceivedMessagesHeaders _receivedMessagesHeaders = new ();
    private readonly ILogger? _logger;
    private OrderCancellingChecker? _orderCancellingChecker;

    public OperationalRobot(
        RobotSettings settings,
        ILogger? logger = null)
    {
        _logger = logger;
        TopicPrefix = settings.RobotTopicPrefix;
        SerialNumber = settings.RobotSerialNumber;
        ObservedTopics = new List<string>()
        {
            $"{settings.RobotTopicPrefix}/connection",
            $"{settings.RobotTopicPrefix}/state",
            $"{settings.RobotTopicPrefix}/factsheet"
        };

        OrderTopic = $"{settings.RobotTopicPrefix}/order";
        InstanActionTopic = $"{settings.RobotTopicPrefix}/instantAction";

        _isObsevingVisualization = settings.ShouldObserveVisualization;
        if (settings.ShouldObserveVisualization)
        {
            ObservedTopics.Add($"{settings.RobotTopicPrefix}/visualization");       
        }
    }
    
    private event EventHandler<RobotPositionChangedEvent>? RobotPositionChanged;
    private event EventHandler<RobotStateChangedEvent>? RobotStateChanged;
    private event EventHandler<RobotOrderStateChangedEvent>? RobotOrderStateChanged;
    private event EventHandler<RobotConnectionStateChangedEvent>? RobotConnectionStateChanged;
    
    public string TopicPrefix { get; }
    public RobotSerialNumber SerialNumber { get; }
    public ICollection<string> ObservedTopics { get; }
    public string OrderTopic { get; }
    public string InstanActionTopic { get; }
    public ConnectionState ConnectionState { get; private set; }
    public FactsheetInfo? Factsheet { get; private set; }
    public FactsheetProtocolInfo? FactsheetProtocolInfo { get; private set; }
    public RobotState? State { get; private set; }
    public RobotOrderState? OrderState { get; private set; }
    public Pose? Pose { get; private set; }
    public bool IsLocalized => Pose is not null;
    
    public ICollection<ErrorSpecifics>? Errors { get; private set; }
    
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
    
    public void AddOrderStateChangeHandler(
        EventHandler<RobotOrderStateChangedEvent> robotOrderStateChangedHandler)
    {
        RobotOrderStateChanged += robotOrderStateChangedHandler;
    }
    
    public void AddConnectionStateChangeHandler(
        EventHandler<RobotConnectionStateChangedEvent> robotConnectionStateChangedHandler)
    {
        RobotConnectionStateChanged += robotConnectionStateChangedHandler;
    }

    internal void OnConnectionMessage(ConnectionMessage connectionMessage)
    {
        if (_receivedMessagesHeaders.TryUpdateConnectionHeaderId(connectionMessage.HeaderId) is false)
        {
            _logger?.LogWarning(
                "Received {message type} message with header id from the past. Received header id: {headerId} while current header id: {currentHeaderId}",
                nameof(ConnectionMessage),
                connectionMessage.HeaderId,
                _receivedMessagesHeaders.ConnectionHeaderId!.Value);
        }

        if (connectionMessage.ConnectionState != ConnectionState)
        {
            var previousConnectionState = ConnectionState;
            ConnectionState = connectionMessage.ConnectionState;
            RobotConnectionStateChanged?.Invoke(
                this,
                new RobotConnectionStateChangedEvent(SerialNumber, previousConnectionState, ConnectionState));
        }
    }
    
    public void OnStateMessage(StateMessage stateMessage)
    {
        if (_receivedMessagesHeaders.TryUpdateStateHeaderId(stateMessage.HeaderId) is false)
        {
            _logger?.LogWarning(
                "Received {message type} message with header id from the past. Received header id: {headerId} while current header id: {currentHeaderId}",
                nameof(StateMessage),
                stateMessage.HeaderId,
                _receivedMessagesHeaders.ConnectionHeaderId!.Value);
        }
        
        if (_isObsevingVisualization is false)
        {
            Pose = Pose.FromMessage(stateMessage.AgvPositionMessage);
        }
        else if (Pose is null)
        {
            _logger?.LogWarning(
                "Observing visualization topic for robot {robotSerialNumber} but no data received about its pose",
                SerialNumber);       
        }

        OrderStatus orderStatus;
        if (_orderCancellingChecker is not null)
        {
            orderStatus = _orderCancellingChecker.HasOrderBeenCanceled(stateMessage) ?
                OrderStatus.Canceled :
                OrderStatus.Canceling;
        }
        else
        {
            var lastNodeState = stateMessage.NodeStates.Last();
            var isFinished = lastNodeState.NodeId == stateMessage.LastNodeId &&
                             lastNodeState.SequenceId == stateMessage.LastNodeSequenceId;
            orderStatus = isFinished ? OrderStatus.Finished : OrderStatus.Pending;
        }

        
        OrderState = RobotOrderState.FromRobotStateMessage(stateMessage, orderStatus);
        RobotOrderStateChanged?.Invoke(
            this,
            new RobotOrderStateChangedEvent(SerialNumber, OrderState));
        
        Errors = stateMessage.Errors?.Select(ErrorSpecifics.FromMessage).ToList();
        State = RobotState.FromMessage(stateMessage);
        RobotStateChanged?.Invoke(
            this,
            new RobotStateChangedEvent(SerialNumber, State));
    }
    
    public void OnFactsheetMessage(FactsheetMessage factsheetMessage)
    {
        if (_receivedMessagesHeaders.TryUpdateFactsheetHeaderId(factsheetMessage.HeaderId) is false)
        {
            _logger?.LogWarning(
                "Received {message type} message with header id from the past. Received header id: {headerId} while current header id: {currentHeaderId}",
                nameof(FactsheetMessage),
                factsheetMessage.HeaderId,
                _receivedMessagesHeaders.ConnectionHeaderId!.Value);
        }
        
        Factsheet = FactsheetInfo.FromMessage(factsheetMessage);
        FactsheetProtocolInfo = FactsheetProtocolInfo.FromMessage(factsheetMessage);
    }
    
    public void OnVisualizationMessage(VisualizationMessage visualizationMessage)
    {
        if (_isObsevingVisualization)
        {
            if (_receivedMessagesHeaders.TryUpdateVisualizationHeaderId(visualizationMessage.HeaderId) is false)
            {
                _logger?.LogWarning(
                    "Received {message type} message with header id from the past. Received header id: {headerId} while current header id: {currentHeaderId}",
                    nameof(VisualizationMessage),
                    visualizationMessage.HeaderId,
                    _receivedMessagesHeaders.ConnectionHeaderId!.Value);
            }
            UpdateRobotPosition(visualizationMessage.AgvPositionMessage);
        }
    }

    public void InitOrderCancellation(OrderId orderId, ActionId cancelActionId)
    {
        _orderCancellingChecker = new OrderCancellingChecker(cancelActionId, orderId);
    }

    private void UpdateRobotPosition(AgvPositionMessage positionMessage)
    {
        Pose = Pose.FromMessage(positionMessage);
        RobotPositionChanged?.Invoke(
            this,
            new RobotPositionChangedEvent(SerialNumber, Pose.X, Pose.Y, Pose.Theta));
    }

}