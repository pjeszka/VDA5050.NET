using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VDA5050.NET.Internal.MQTT.Topics;
using VDA5050.NET.Internal.VdaDomain.Robots;
using VDA5050.NET.Public.DependencyInjection.Settings;

namespace VDA5050.NET.Internal.MQTT.BackgroundServices;

internal sealed class MqttReconnectionBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceScope;
    private readonly IMqttConnection _connection;
    private readonly TimeSpan _reconnectInterval;
    private readonly ILogger<MqttReconnectionBackgroundService> _logger;
    private readonly string _brokerAddress;

    public MqttReconnectionBackgroundService(
        IMqttConnection connection,
        Vda5050MasterSettings masterSettings,
        ILogger<MqttReconnectionBackgroundService> logger,
        IServiceProvider serviceScope)
    {
        _connection = connection;
        _logger = logger;
        _serviceScope = serviceScope;
        var mqttConnectionSettings = masterSettings.Mqtt;
        _reconnectInterval = TimeSpan.FromMilliseconds(mqttConnectionSettings.ReconnectionPeriodInMs);
        _brokerAddress = mqttConnectionSettings.BrokerAddress + ":" + mqttConnectionSettings.Port;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var timer = new PeriodicTimer(_reconnectInterval);
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                if (_connection.IsConnected is false)
                {
                    using var scope = _serviceScope.CreateScope();
                    var topicsProvider = scope.ServiceProvider
                        .GetRequiredService<ISubscribedTopicsProvider>();
                    var topicsToSubscribe = await topicsProvider.GetTopicsToSubscribe();
                    foreach (var topic in topicsToSubscribe)
                    {
                        if (_connection.HasSubscriber(topic) is false)
                        {
                            await _connection.AddSubscription(topic);
                        }
                    }

                    _logger?.LogInformation(
                        "Reconnecting to MQTT broker for VDA5050 with address: {brokerAddress}. ",
                        _brokerAddress);
                    await _connection.ConnectAsync();
                }
            }
            catch (Exception e)
            {
                _logger?.LogCritical(e.Message, e.StackTrace);
            }
        }
    }
}
