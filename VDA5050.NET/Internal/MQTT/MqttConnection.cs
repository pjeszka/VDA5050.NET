using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Extensions.ManagedClient;
using VDA5050.NET.Internal.MQTT.Settings;

namespace VDA5050.NET.Internal.MQTT;

public sealed class MqttConnection : IMqttConnection
{
    private const string ClientId = "triggers_mqtt";
    private readonly IManagedMqttClient _client;
    private readonly ManagedMqttClientOptions _clientOptions;
    private readonly ILogger<MqttConnection>? _logger;
    private readonly List<string> _topics = new ();
    private readonly IMessageDispatcher _messageDispatcher;

    private bool _connectionRequested;
    private string? _connectionFailedErrorMessage;
    private DateTime? _lastConnectedAt;

    public MqttConnection(
        IOptions<MqttConnectionSettings> connectionConfig,
        IMessageDispatcher messageDispatcher,
        ILogger<MqttConnection>? logger = null)
    {
        _messageDispatcher = messageDispatcher;
        _logger = logger;

        var mqttFactory = new MqttFactory();
        _client = mqttFactory.CreateManagedMqttClient();

        var mqttClientOptions = new MqttClientOptionsBuilder()
            .WithCleanSession()
            .WithTcpServer(connectionConfig.Value.BrokerAddress, connectionConfig.Value.Port)
            .WithCredentials(connectionConfig.Value.Username, connectionConfig.Value.Password)
            .WithClientId(ClientId)
            .Build();

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

        await _client.StartAsync(_clientOptions);
        await SubscribeAllDefinedTopics();
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
}
