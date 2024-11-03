using Domain.Core.EventBus;
using Microsoft.Extensions.DependencyInjection;

namespace MessagingService;

public static class DependencyInjection
{
    public static void AddRabbitMQEventBus(this IServiceCollection services)
    {
        services.AddSingleton<IRabbitMQConnectionService, RabbitMQConnectionService>();

        services.AddSingleton<IEventBus, RabbitMQEventBus>(sp =>
        {
            var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
            var connectionService = sp.GetRequiredService<IRabbitMQConnectionService>();
            return new RabbitMQEventBus(scopeFactory, connectionService);
        });
    }
}