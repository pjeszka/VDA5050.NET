using Microsoft.Extensions.DependencyInjection;
using VDA5050.NET.Internal.MQTT.BackgroundServices;

namespace VDA5050.NET.Internal.MQTT;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMqtt(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<IMessageDispatcher, MessageDispatcher>()
            .AddSingleton<IMqttConnection, MqttConnection>()
            .AddHostedService<MqttReconnectionBackgroundService>();
        return serviceCollection;
    }
}