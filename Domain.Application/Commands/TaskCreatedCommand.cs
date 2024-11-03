using Domain.Core.Commands;

namespace TaskService.Commands;

public class TaskCreatedCommand(int taskId, string taskName, string taskDescription) : Command
{
    public int TaskId { get; set; } = taskId;
    public string TaskName { get; set; } = taskName;
    public string TaskDescription { get; set; } = taskDescription;
}