using Domain.Core.EventBus;
using TaskEventsTracker.Events;

namespace TaskEventsTracker.EventHandlers;

public class TaskCreateEventHandler(IEventBus eventBus) : IEventHandler<TaskCreateEvent>
{
    public Task Handle(TaskCreateEvent @event)
    {
        Console.WriteLine($"Task created: Id: {@event.TaskId}, Name: {@event.TaskName}, Description: {@event.TaskDescription}");
        
        eventBus.Publish(new TaskCreateEvent(@event.TaskId, @event.TaskName, @event.TaskDescription));
        
        return Task.CompletedTask;
    }
}