using Domain.Core.EventBus;
using MediatR;
using TaskService.Commands;
using TaskService.Events;

namespace TaskService.CommandHandlers;

public class TaskCreatedCommandHandler(IEventBus bus) : IRequestHandler<TaskCreatedCommand, bool>
{
    public async Task<bool> Handle(TaskCreatedCommand command, CancellationToken cancellationToken)
    {
        await bus.Publish(new TaskCreateEvent(command.TaskId, command.TaskName, command.TaskDescription));
        return true;
    }
}