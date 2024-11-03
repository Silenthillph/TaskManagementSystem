using Domain.Core.EventBus;
using MediatR;
using TaskEventsTracker.Commands;
using TaskEventsTracker.Events;

namespace TaskEventsTracker.CommandHandlers;

public class TaskStatusChangeActionCommandHandler(IEventBus bus) : IRequestHandler<TaskStatusChangeActionCommand, bool>
{
    public async Task<bool> Handle(TaskStatusChangeActionCommand command, CancellationToken cancellationToken)
    {
        await bus.Publish(new TaskStatusChangeActionEvent(command.TaskId));
        return true;
    }
}