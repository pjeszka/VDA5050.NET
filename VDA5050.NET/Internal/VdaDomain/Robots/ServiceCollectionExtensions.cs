using Microsoft.Extensions.DependencyInjection;

namespace VDA5050.NET.Internal.VdaDomain.Robots;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRobotManagement(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<IConnectedRobotRepository, ConnectedRobotRepository>();
        return serviceCollection;
    }
}