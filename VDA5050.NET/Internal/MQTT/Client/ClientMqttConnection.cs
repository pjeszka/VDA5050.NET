using System.Text;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using MQTTnet.Protocol;
using VDA5050.NET.Internal.MQTT.Topics;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts.Connection;
using VDA5050.NET.Public.DependencyInjection.Settings;
using VDA5050.NET.Public.Models.MQTT;
using VDA5050.NET.Public.Services;

namespace VDA5050.NET.Internal.MQTT.Client;

internal sealed class ClientMqttConnection : IMqttConnection
{
    private const string ClientId = "vda5050_client";
    private readonly IManagedMqttClient _client;
    private ManagedMqttClientOptions _clientOptions;
    private readonly ILogger<ClientMqttConnection>? _logger;
    private readonly List<string> _topics = new ();
    private readonly IMessageDispatcher _messageDispatcher;
    private readonly ISystemClock _systemClock;
    private readonly Vda5050ClientSettings _config;
    
    private bool _connectionRequested;
    private string? _connectionFailedErrorMessage;
    private DateTime? _lastConnectedAt;
    private uint _connectionMessageHeaderId = 1;
    
    public ClientMqttConnection(
        Vda5050ClientSettings clientSettings,
        IMessageDispatcher messageDispatcher,
        ISystemClock systemClock,
        ILogger<ClientMqttConnection>? logger = null)
    {
        _messageDispatcher = messageDispatcher;
        _systemClock = systemClock;
        _logger = logger;

        var mqttFactory = new MqttFactory();
        _client = mqttFactory.CreateManagedMqttClient();
        
        _config = clientSettings;
        
        var mqttClientOptions = BuildOptions();
        
        
        _client.ApplicationMessageReceivedAsync += async messageArguments => await HandleMessage(messageArguments);
        _client.ConnectedAsync += HandleSuccessfulConnection;
        _client.ConnectingFailedAsync += HandleFailedConnection;
        _client.DisconnectedAsync += HandleDisconnection;

        _clientOptions = new ManagedMqttClientOptionsBuilder()
            .WithClientOptions(mqttClientOptions)
            .Build();
    }

    public bool IsConnected { get; private set; }

    public bool HasSubscriber(string topic)
    {
        return _topics.Contains(topic);
    }

    public async Task ConnectAsync()
    {
        ClearConnectionError();

        if (_client.IsConnected || _connectionRequested)
        {
            return;
        }

        _connectionRequested = true;
        
        var mqttClientOptions = BuildOptions();
        
        _clientOptions = new ManagedMqttClientOptionsBuilder()
            .WithClientOptions(mqttClientOptions)
            .Build();

        await _client.StartAsync(_clientOptions);
        await PublishAsync(
            _clientOptions.ClientOptions.WillTopic,
            ConnectionMessage.CreateOnlineMessage(_connectionMessageHeaderId, _config.Robot.RobotTopicPrefix, _systemClock.Now).ToJson(),
            isRetained: true,
            qos: MqttQualityOfServiceLevel.AtLeastOnce
        );
        _connectionMessageHeaderId++;
        await SubscribeAllDefinedTopics();
    }

    public Task PublishAsync(
        string topic,
        string messagePayload,
        bool isRetained = false,
        MqttQualityOfServiceLevel qos = MqttQualityOfServiceLevel.AtMostOnce)
    {
        var message = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(Encoding.UTF8.GetBytes(messagePayload))
            .WithQualityOfServiceLevel(qos)
            .WithRetainFlag(isRetained)
            .Build();
        
        return _client.EnqueueAsync(message);
    }

    public MqttConnectionDetailsDto GetConnectionDetails()
    {
        return new MqttConnectionDetailsDto(
            IsConnected,
            _connectionFailedErrorMessage,
            _lastConnectedAt,
            _topics);
    }

    public async Task AddSubscription(string topic)
    {
        if (_topics.Contains(topic))
        {
            return;
        }

        await _client.SubscribeAsync(topic);
        _topics.Add(topic);
    }

    public async Task RemoveSubscription(string topic)
    {
        if (_topics.Contains(topic) is false)
        {
            return;
        }

        await _client.UnsubscribeAsync(topic);
        _topics.Remove(topic);
    }

    private void ClearConnectionError()
    {
        _connectionFailedErrorMessage = null;
    }

    private async Task SubscribeAllDefinedTopics()
    {
        if (_topics.Any() is false)
        {
            return;
        }

        foreach (var topic in _topics)
        {
            await _client.SubscribeAsync(topic);
        }
    }

    private async Task HandleMessage(MqttApplicationMessageReceivedEventArgs arguments)
    {
        var messageJson = arguments.ApplicationMessage.ConvertPayloadToString();
        await _messageDispatcher.DispatchMessage(arguments.ApplicationMessage.Topic, messageJson);
    }

    private Task HandleSuccessfulConnection(MqttClientConnectedEventArgs arguments)
    {
        _connectionRequested = false;
        _lastConnectedAt = DateTime.UtcNow;
        IsConnected = true;
        ClearConnectionError();

        _logger?.LogInformation(
            "Connected to MQTT broker with address '{mqttBrokerAddress}'",
            _client.Options.ClientOptions.ChannelOptions.ToString());

        return Task.CompletedTask;
    }

    private Task HandleDisconnection(MqttClientDisconnectedEventArgs arguments)
    {
        IsConnected = false;
        _logger?.LogWarning(
            "Disconnected from MQTT broker with address '{mqttBrokerAddress}'",
            _client.Options.ClientOptions.ChannelOptions.ToString());
        var connectionError = arguments.Exception?.InnerException?.Message ?? arguments.Exception?.Message;

        if (string.IsNullOrWhiteSpace(connectionError))
        {
            return Task.CompletedTask;
        }

        SetConnectionError(connectionError);

        if (IsBadUserNameOrPassword(arguments.Exception!) is false)
        {
            _connectionRequested = false;
            _ = Task.Factory.StartNew(async () =>
            {
                await _client.StopAsync();
            });
        }

        return Task.CompletedTask;
    }

    private Task HandleFailedConnection(ConnectingFailedEventArgs arguments)
    {
        var connectionError = arguments.Exception.InnerException?.Message ?? arguments.Exception.Message;
        SetConnectionError(connectionError);

        _logger?.LogError(
          "Connection to MQTT broker with address '{mqttBrokerAddress}' failed. " +
          "Reason: {mqttConnectionFailedReason}",
          _client.Options.ClientOptions.ChannelOptions.ToString(),
          connectionError);

        if (IsBadUserNameOrPassword(arguments.Exception))
        {
            _connectionRequested = false;
            _ = Task.Factory.StartNew(async () =>
            {
                await _client.StopAsync();
            });
        }

        return Task.CompletedTask;
    }

    private void SetConnectionError(string errorMessage)
    {
        IsConnected = false;
        _connectionFailedErrorMessage = errorMessage;
    }

    private bool IsBadUserNameOrPassword(Exception exception)
    {
        var invalidCredentialsCodeName = nameof(MqttClientConnectResultCode.BadUserNameOrPassword);

        return exception.Message.Contains(invalidCredentialsCodeName);
    }

    private string CreateLastWillMessage(Vda5050ClientSettings clientSettings)
    {
        var lastWillHeaderId = _connectionMessageHeaderId + 1; // next header id for last will message
        var lastWillMessage = ConnectionMessage.CreateConnectionBrokenMessage(
            lastWillHeaderId,
            clientSettings.Robot.RobotTopicPrefix,
            _systemClock.Now);

        return lastWillMessage.ToJson();
    }
    
    private MqttClientOptions BuildOptions()
    {
        var connectionConfig = _config.Mqtt;

        var mqttClientOptionsBuilder = new MqttClientOptionsBuilder()
            .WithCleanSession()
            .WithTcpServer(connectionConfig.BrokerAddress, connectionConfig.Port);
        
        if (string.IsNullOrWhiteSpace(connectionConfig.Username) is false &&
            string.IsNullOrWhiteSpace(connectionConfig.Password) is false)
        {
            mqttClientOptionsBuilder.WithCredentials(connectionConfig.Username, connectionConfig.Password);
        }
        
        var clientId = string.IsNullOrWhiteSpace(connectionConfig.ClientId) is false ?
            connectionConfig.ClientId :
            ClientId;

        mqttClientOptionsBuilder.WithClientId(clientId);
        
        var lastWillMessage = CreateLastWillMessage(_config);

        mqttClientOptionsBuilder
            .WithWillTopic(
                $"{_config.Robot.RobotTopicPrefix}/connection")
            .WithWillRetain(true)
            .WithWillPayload(lastWillMessage)
            .WithWillQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce);
        
        var mqttClientOptions = mqttClientOptionsBuilder.Build();

        return mqttClientOptions;
    }
}
