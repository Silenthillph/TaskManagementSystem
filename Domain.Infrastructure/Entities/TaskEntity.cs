using System.ComponentModel.DataAnnotations.Schema;
using TaskStatus = Domain.Core.Enums.TaskStatus;

namespace Domain.Infrastructure.Entities;

[Table("Task")]
public class TaskEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public TaskStatus Status { get; set; }
    public string? AssignedTo { get; set; }
}
