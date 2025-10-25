using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using VDA5050.NET.Internal.VdaDomain.Robots;

namespace VDA5050.NET.Internal.MQTT;

public sealed class MessageDispatcher : IMessageDispatcher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<IMessageDispatcher> _logger;

    public MessageDispatcher(
        IServiceProvider serviceProvider,
        ILogger<IMessageDispatcher> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task DispatchMessage(string topic, string message)
    {
        var messageObject = JObject.Parse(message);
        using var scope = _serviceProvider.CreateScope();
        
        var robotRepository = scope.ServiceProvider.GetRequiredService<IRobotRepository>();
        
        //var topicMapper = robotRepository.GetTopicsForRobot()
    }
}
