using Domain.Core.Commands;
using TaskStatus = Domain.Core.Enums.TaskStatus;

namespace TaskService.Commands;

public class TaskStatusUpdatedCommand(int taskId, TaskStatus newStatus): Command
{
    public int TaskId { get; set; } = taskId;
    public TaskStatus TaskNewStatus { get; set; } = newStatus;
}