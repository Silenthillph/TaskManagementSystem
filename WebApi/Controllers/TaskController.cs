using Microsoft.AspNetCore.Mvc;
using TaskService;
using TaskService.Models;
using TaskStatus = Domain.Core.Enums.TaskStatus;

namespace WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TaskController(ITaskService taskService) : ControllerBase
{
    /// <summary>
    /// Get all tasks.
    /// </summary>
    [HttpGet("[action]")]
    public async Task<ActionResult<IEnumerable<TaskModel>>> GetAll()
    {
        var tasks = await taskService.GetAllTasks();
        return Ok(tasks);
    }
    
    /// <summary>
    /// Add a new task.
    /// </summary>
    [HttpPost("[action]")]
    public async Task<ActionResult<TaskModel>> Add(TaskModelBase taskModel)
    {
        var createdTask = await taskService.AddTask(taskModel);
        return Ok(createdTask);
    }

    /// <summary>
    /// Update the status of a task.
    /// </summary>
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateTaskStatus(int id, [FromBody] TaskStatus status)
    {
        if (!Enum.IsDefined(typeof(TaskStatus), status))
        {
            return BadRequest("Invalid status value.");
        }

        var result = await taskService.UpdateTaskStatus(id, status);
        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}