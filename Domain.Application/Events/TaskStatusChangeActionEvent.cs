using Domain.Core.Events;

namespace TaskService.Events;

public class TaskStatusChangeActionEvent(int taskId): Event
{
    public int TaskId { get; set; } = taskId;
}