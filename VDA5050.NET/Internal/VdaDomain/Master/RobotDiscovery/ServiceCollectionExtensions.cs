using Microsoft.Extensions.DependencyInjection;
using VDA5050.NET.Internal.MQTT.Topics;
using VDA5050.NET.Internal.VdaDomain.Master.RobotDiscovery;

namespace VDA5050.NET.Internal.VdaDomain.RobotDiscovery;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRobotDiscovery(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<IInitialTopicsHandler, RobotDiscoveryService>()
            .AddSingleton<IDiscoveredRobotRepository, DiscoveredRobotRepository>();
        
        return serviceCollection;
    }

}