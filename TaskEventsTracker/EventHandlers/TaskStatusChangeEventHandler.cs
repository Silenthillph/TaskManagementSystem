using Domain.Core.EventBus;
using MediatR;
using TaskEventsTracker.Commands;
using TaskEventsTracker.Events;

namespace TaskEventsTracker.EventHandlers;

public class TaskStatusChangeEventHandler(IMediator mediator) : IEventHandler<TaskStatusChangeEvent>
{
    public Task Handle(TaskStatusChangeEvent @event)
    {
        Console.WriteLine($"Task status updated: Id: {@event.TaskId}, new status: {@event.TaskNewStatus.ToString()}");

        // check if retry policy works
        // if (@event.TaskId % 2 == 0)
        // {
        //     throw new Exception();
        // }
        
        mediator.Send(new TaskStatusChangeActionCommand(@event.TaskId));
        
        return Task.CompletedTask;
    }
}