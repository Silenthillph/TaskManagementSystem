using Domain.Core.EventBus;
using TaskService.Events;

namespace TaskService.EventHandlers;

public class TaskStatusChangeActionEventHandler: IEventHandler<TaskStatusChangeActionEvent>
{
    public Task Handle(TaskStatusChangeActionEvent @event)
    {
        Console.WriteLine($"Task Id : {@event.TaskId}, status update action executed");
        return Task.CompletedTask;
    }
}