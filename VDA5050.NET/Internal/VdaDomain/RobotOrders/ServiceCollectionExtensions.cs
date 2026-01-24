using Microsoft.Extensions.DependencyInjection;
using VDA5050.NET.Internal.VdaDomain.InstantActions;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts;
using VDA5050.NET.Internal.VdaDomain.RobotOrders.OrderRequesting;

namespace VDA5050.NET.Internal.VdaDomain.RobotOrders;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRobotOrders(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<MessageBuilder>()
            .AddSingleton<IRobotOrderRequestStateRepository, RobotOrderRequestStateRepository>()
            .AddSingleton<IRobotOrderSender, RobotOrderSender>()
            .AddSingleton<IRobotInstantActionsSender, RobotInstantActionsSender>()
            .AddSingleton<IInstantActionRequestRepository, InstantActionRequestRepository>();
        return serviceCollection;
    }
}