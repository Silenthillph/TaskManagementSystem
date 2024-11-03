using Domain.Core.EventBus;
using MediatR;
using TaskService.Commands;
using TaskService.Events;

namespace TaskService.CommandHandlers;

public class TaskStatusUpdatedCommandHandler(IEventBus bus) : IRequestHandler<TaskStatusUpdatedCommand, bool>
{
    public async Task<bool> Handle(TaskStatusUpdatedCommand command, CancellationToken cancellationToken)
    {
        await bus.Publish(new TaskStatusChangeEvent(command.TaskId, command.TaskNewStatus));
        return true;
    }
}