using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VDA5050.NET.Internal.MQTT.Settings;
using VDA5050.NET.Internal.VdaDomain.Robots;

namespace VDA5050.NET.Internal.MQTT.BackgroundServices;

public sealed class MqttReconnectionBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceScope;
    private readonly IMqttConnection _connection;
    private readonly TimeSpan _reconnectInterval;
    private readonly ILogger<MqttReconnectionBackgroundService> _logger;
    private readonly string _brokerAddress;

    public MqttReconnectionBackgroundService(
        IMqttConnection connection,
        IOptions<MqttConnectionSettings> mqttConnectionSettings,
        ILogger<MqttReconnectionBackgroundService> logger,
        IServiceProvider serviceScope)
    {
        _connection = connection;
        _logger = logger;
        _serviceScope = serviceScope;
        _reconnectInterval = TimeSpan.FromMilliseconds(mqttConnectionSettings.Value.ReconnectionPeriodInMs);
        _brokerAddress = mqttConnectionSettings.Value.BrokerAddress + ":" + mqttConnectionSettings.Value.Port;
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
                    var robotRepository = scope.ServiceProvider
                        .GetRequiredService<IRobotRepository>();
                    var robots = robotRepository
                        .GetRobotNames();
                    foreach (var robot in robots)
                    {
                        foreach (var robotTopic in robotRepository.GetTopicsForRobot(robot))
                        {
                            if (_connection.HasSubscriber(robotTopic) is false)
                            {
                                await _connection.AddSubscription(robotTopic);
                            }
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
