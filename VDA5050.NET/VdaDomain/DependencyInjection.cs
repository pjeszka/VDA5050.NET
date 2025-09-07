using Microsoft.Extensions.DependencyInjection;
using VDA5050.NET.API.Services;
using VDA5050.NET.MQTT;
using VDA5050.NET.MQTT.BackgroundServices;
using VDA5050.NET.VdaDomain.Master;
using VDA5050.NET.VdaDomain.Robots;

namespace VDA5050.NET.VdaDomain;

public static class DependencyInjection
{
    public static IServiceCollection AddVda5050Master(
        this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<IVda5050Master, Vda5050Master>()
            .AddSingleton<IMessageDispatcher, MessageDispatcher>()
            .AddSingleton<IRobotRepository, RobotRepository>()
            .AddHostedService<MqttReconnectionBackgroundService>();
    }
}