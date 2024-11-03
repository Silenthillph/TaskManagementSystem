using Domain.Core.Events;
using TaskStatus = Domain.Core.Enums.TaskStatus;

namespace TaskEventsTracker.Events;

public class TaskStatusChangeEvent(int taskId, TaskStatus newStatus): Event
{
    public int TaskId { get; set; } = taskId;
    public TaskStatus TaskNewStatus { get; set; } = newStatus;
}