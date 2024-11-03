using Domain.Core.Commands;

namespace TaskEventsTracker.Commands;

public class TaskStatusChangeActionCommand(int taskId): Command
{
    public int TaskId { get; set; } = taskId;
}