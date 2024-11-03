using AutoMapper;
using Domain.Core.EventBus;
using Domain.Infrastructure.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskService.Commands;
using TaskService.Models;
using TaskStatus = Domain.Core.Enums.TaskStatus;

namespace TaskService;

public class TaskService(IAppDbContext ctx, IMapper mapper, IMediator mediator) : ITaskService
{
    public async Task<IEnumerable<TaskModel>> GetAllTasks()
    {
        var tasks = await ctx.Set<TaskEntity>().ToListAsync();
        
        return mapper.Map<List<TaskModel>>(tasks);
    }

    public async Task<TaskModel> AddTask(TaskModelBase taskModel)
    {
        var taskEntity = mapper.Map<TaskEntity>(taskModel);
        await ctx.Set<TaskEntity>().AddAsync(taskEntity);
        await ctx.SaveChangesAsync();
        
        await mediator.Send(new TaskCreatedCommand(taskEntity.Id, taskEntity.Name, taskEntity.Description));
        
        return mapper.Map<TaskModel>(taskEntity);
    }

    public async Task<bool> UpdateTaskStatus(int taskId, TaskStatus newStatus)
    {
        var taskEntity = await ctx.Set<TaskEntity>().FindAsync(taskId);
        if (taskEntity == null) return false;

        taskEntity.Status = newStatus;
        await ctx.SaveChangesAsync();
        
        await mediator.Send(new TaskStatusUpdatedCommand(taskEntity.Id, taskEntity.Status));
        
        return true;
    }
}