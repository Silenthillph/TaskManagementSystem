using System.Reflection;
using Domain.Core.EventBus;
using MessagingService;
using Microsoft.Extensions.DependencyInjection;
using TaskEventsTracker.EventHandlers;
using TaskEventsTracker.Events;

public static class Program
{
    static async Task Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddRabbitMQEventBus();
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            
        services.AddTransient<TaskCreateEventHandler>();
        services.AddTransient<TaskStatusChangeEventHandler>();
        services.AddTransient<TaskStatusChangeEventHandler>();

        //Domain Events
        services.AddTransient<IEventHandler<TaskCreateEvent>, TaskCreateEventHandler>();
        services.AddTransient<IEventHandler<TaskStatusChangeEvent>, TaskStatusChangeEventHandler>();
        
        var serviceProvider = services.BuildServiceProvider();
        
        var eventBus = serviceProvider.GetRequiredService<IEventBus>();
        
        await eventBus.Subscribe<TaskCreateEvent, TaskCreateEventHandler>();
        await eventBus.Subscribe<TaskStatusChangeEvent, TaskStatusChangeEventHandler>();

        Console.ReadLine();
    }
}