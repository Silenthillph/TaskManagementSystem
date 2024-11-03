using Domain.Core.EventBus;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace MessagingService;

public static class DependencyInjection
{
    public static void AddRabbitMQEventBus(this IServiceCollection services)
    {
        services.AddSingleton<IEventBus, RabbitMQEventBus>(sp =>
        {
            var scopeFactory = sp.GetRequiredService<IServiceScopeFactory>();
            return new RabbitMQEventBus(scopeFactory);
        });

    }
}