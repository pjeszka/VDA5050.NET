using Microsoft.Extensions.DependencyInjection;
using VDA5050.NET.Internal.MQTT.BackgroundServices;
using VDA5050.NET.Internal.MQTT.Client;

namespace VDA5050.NET.Internal.MQTT;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMasterMqtt(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<IMqttConnection, MasterMqttConnection>()
            .AddHostedService<MqttReconnectionBackgroundService>();
        return serviceCollection;
    }
    
    public static IServiceCollection AddClientMqtt(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<IMqttConnection, ClientMqttConnection>()
            .AddHostedService<MqttReconnectionBackgroundService>();
        return serviceCollection;
    }
}