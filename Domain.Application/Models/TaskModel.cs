using System.Text.Json.Serialization;
using TaskStatus = Domain.Core.Enums.TaskStatus;

namespace TaskService.Models;

public class TaskModelBase
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string AssignedTo { get; set; }
}

public class TaskModel: TaskModelBase
{
    public int Id { get; set; }
    public TaskStatus Status { get; set; }
}