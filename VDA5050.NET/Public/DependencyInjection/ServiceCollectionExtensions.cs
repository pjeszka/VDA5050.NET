using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VDA5050.NET.Internal.MQTT;
using VDA5050.NET.Internal.MQTT.BackgroundServices;
using VDA5050.NET.Internal.System;
using VDA5050.NET.Internal.VdaDomain.Master;
using VDA5050.NET.Internal.VdaDomain.RobotDiscovery;
using VDA5050.NET.Internal.VdaDomain.RobotOrders;
using VDA5050.NET.Internal.VdaDomain.RobotOrders.OrderRequesting;
using VDA5050.NET.Internal.VdaDomain.Robots;
using VDA5050.NET.Public.DependencyInjection.Settings;
using VDA5050.NET.Public.Services;

namespace VDA5050.NET.Public.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddVda5050Master(
        this IServiceCollection serviceCollection,
        IConfiguration configuration,
        ISystemClock? systemClock = null)
    {
        ApplySettings(serviceCollection, configuration);

        if (systemClock is not null)
        {
            serviceCollection.AddSingleton<ISystemClock>(systemClock);
        }
        else
        {
            serviceCollection.AddSingleton<ISystemClock, UtcSystemClock>();
        }

        serviceCollection
            .AddMqtt()
            .AddRobotDiscovery()
            .AddRobotManagement()
            .AddRobotOrders()
            .AddSingleton<IVda5050Master, Vda5050Master>();

        return serviceCollection;
    }

    private static void ApplySettings(IServiceCollection serviceCollection, IConfiguration configuration)
    {
        var settings = new Vda5050MasterSettings();

        var section = configuration.GetSection(Vda5050MasterSettings.SectionName);
        if (section.Exists())
        {
            section.Bind(settings);
        }
            
        serviceCollection.AddSingleton(settings);
    }
}