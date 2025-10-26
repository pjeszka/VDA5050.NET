using Microsoft.Extensions.DependencyInjection;
using VDA5050.NET.Internal.MQTT;
using VDA5050.NET.Internal.MQTT.Topics;

namespace VDA5050.NET.Internal.VdaDomain.Robots;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRobotManagement(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<OperationalRobotRepository>()
            .AddSingleton<IMessageDispatcher, RobotMessageDispatcher>()
            .AddSingleton<IOperationalRobotRepository>(sp => sp.GetRequiredService<OperationalRobotRepository>())
            .AddSingleton<ISubscribedTopicsProvider>(sp => sp.GetRequiredService<OperationalRobotRepository>());
        return serviceCollection;
    }
}