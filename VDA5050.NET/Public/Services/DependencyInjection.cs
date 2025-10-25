using Microsoft.Extensions.DependencyInjection;
using VDA5050.NET.Internal.MQTT;
using VDA5050.NET.Internal.MQTT.BackgroundServices;
using VDA5050.NET.Internal.VdaDomain.Master;
using VDA5050.NET.Internal.VdaDomain.Robots;

namespace VDA5050.NET.Public.Services;

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

        return serviceCollection;
    }
}