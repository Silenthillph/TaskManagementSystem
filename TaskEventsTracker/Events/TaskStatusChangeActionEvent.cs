using Domain.Core.Events;

namespace TaskEventsTracker.Events;

public class TaskStatusChangeActionEvent(int taskId): Event
{
    public int TaskId { get; set; } = taskId;
}