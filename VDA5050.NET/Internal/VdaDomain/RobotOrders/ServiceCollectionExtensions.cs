using Microsoft.Extensions.DependencyInjection;
using VDA5050.NET.Internal.VdaDomain.Messages.MessageContracts;
using VDA5050.NET.Internal.VdaDomain.RobotOrders.OrderRequesting;

namespace VDA5050.NET.Internal.VdaDomain.RobotOrders;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRobotOrders(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddSingleton<MessageBuilder>()
            .AddSingleton<IRobotOrderSender, RobotOrderSender>();
        return serviceCollection;
    }
}