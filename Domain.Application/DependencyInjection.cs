using System.Reflection;
using Domain.Core.EventBus;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using TaskService.CommandHandlers;
using TaskService.Commands;
using TaskService.EventHandlers;
using TaskService.Events;

namespace TaskService;

public static class DependencyInjection
{
    public static IServiceCollection AddTaskService(this IServiceCollection services)
    {
        services.AddScoped<ITaskService, TaskService>();

        services.AddTransient<TaskStatusChangeActionEventHandler>();
        services.AddTransient<IEventHandler<TaskStatusChangeActionEvent>, TaskStatusChangeActionEventHandler>();    
        
        services.AddTransient<IRequestHandler<TaskCreatedCommand, bool>, TaskCreatedCommandHandler>();
        services.AddTransient<IRequestHandler<TaskStatusUpdatedCommand, bool>, TaskStatusUpdatedCommandHandler>();
        
        // Automatically register all profiles in the Domain.Application assembly
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        
        return services;
    }
}