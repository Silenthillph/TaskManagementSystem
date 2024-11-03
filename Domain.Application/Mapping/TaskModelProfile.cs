using AutoMapper;
using Domain.Infrastructure.Entities;
using TaskService.Models;

namespace TaskService.Mapping;

public class TaskModelProfile: Profile
{
    public TaskModelProfile()
    {
        CreateMap<TaskEntity, TaskModel>()
            .ForMember(d => d.Status, s => s.MapFrom(src => (TaskStatus)src.Status));

        CreateMap<TaskModelBase, TaskEntity>()
            .ForMember(d => d.Id, s => s.Ignore());
    }
}