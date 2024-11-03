using TaskService.Models;
using TaskStatus = Domain.Core.Enums.TaskStatus;

namespace TaskService;

public interface ITaskService
{
    /// <summary>
    /// Adds a new task.
    /// </summary>
    /// <param name="taskModel">The task model to add.</param>
    /// <returns>The added task.</returns>
    Task<TaskModel> AddTask(TaskModelBase taskModel);

    /// <summary>
    /// Updates the status of a task.
    /// </summary>
    /// <param name="taskId">The ID of the task to update.</param>
    /// <param name="newStatus">The new status of the task.</param>
    /// <returns>True if the update was successful, otherwise false.</returns>
    Task<bool> UpdateTaskStatus(int taskId, TaskStatus newStatus);

    /// <summary>
    /// Retrieves a list of all tasks with their statuses.
    /// </summary>
    /// <returns>A list of tasks with all their details.</returns>
    Task<IEnumerable<TaskModel>> GetAllTasks();
}