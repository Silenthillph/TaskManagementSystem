using AutoMapper;
using Domain.Infrastructure.Context;
using Domain.Infrastructure.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskService.Commands;
using TaskService.Models;
using TaskStatus = Domain.Core.Enums.TaskStatus;

namespace Domain.Application.Tests.Services;

public class TaskServiceTests
{
    private readonly Mock<IAppDbContext> _mockContext;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IMediator> _mockMediator;
    private readonly TaskService.TaskService _taskService;

    public TaskServiceTests()
    {
        _mockContext = new Mock<IAppDbContext>();
        _mockMapper = new Mock<IMapper>();
        _mockMediator = new Mock<IMediator>();
        _taskService = new TaskService.TaskService(_mockContext.Object, _mockMapper.Object, _mockMediator.Object);
    }

    [Fact]
    public async Task AddTask_ShouldAddTaskAndSendCommand()
    {
        // Arrange
        var taskModel = new TaskModelBase { Name = "Test Task", Description = "Test Description" };
        var taskEntity = new TaskEntity { Id = 1, Name = "Test Task", Description = "Test Description" };
        var taskModelMapped = new TaskModel { Id = 1, Name = "Test Task", Description = "Test Description" };

        _mockMapper.Setup(m => m.Map<TaskEntity>(taskModel)).Returns(taskEntity);
        _mockMapper.Setup(m => m.Map<TaskModel>(taskEntity)).Returns(taskModelMapped);

        var dbSetMock = new Mock<DbSet<TaskEntity>>();
        _mockContext.Setup(c => c.Set<TaskEntity>()).Returns(dbSetMock.Object);

        // Act
        var result = await _taskService.AddTask(taskModel);

        // Assert
        _mockContext.Verify(c => c.Set<TaskEntity>().AddAsync(It.IsAny<TaskEntity>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockMediator.Verify(m => m.Send(It.IsAny<TaskCreatedCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.Equal(taskModelMapped, result);
    }

    [Fact]
    public async Task UpdateTaskStatus_ShouldUpdateStatusAndSendCommand()
    {
        // Arrange
        int taskId = 1;
        var newStatus = TaskStatus.Completed;
        var taskEntity = new TaskEntity { Id = taskId, Status = TaskStatus.InProgress };

        var dbSetMock = new Mock<DbSet<TaskEntity>>();
        dbSetMock.Setup(m => m.FindAsync(taskId)).ReturnsAsync(taskEntity);
        _mockContext.Setup(c => c.Set<TaskEntity>()).Returns(dbSetMock.Object);

        // Act
        var result = await _taskService.UpdateTaskStatus(taskId, newStatus);

        // Assert
        Assert.True(result);
        Assert.Equal(newStatus, taskEntity.Status);
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _mockMediator.Verify(m => m.Send(It.IsAny<TaskStatusUpdatedCommand>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateTaskStatus_ShouldReturnFalseIfTaskNotFound()
    {
        // Arrange
        int taskId = 1;
        var newStatus = TaskStatus.Completed;

        var dbSetMock = new Mock<DbSet<TaskEntity>>();
        dbSetMock.Setup(m => m.FindAsync(taskId)).ReturnsAsync((TaskEntity)null);
        _mockContext.Setup(c => c.Set<TaskEntity>()).Returns(dbSetMock.Object);

        // Act
        var result = await _taskService.UpdateTaskStatus(taskId, newStatus);

        // Assert
        Assert.False(result);
        _mockContext.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        _mockMediator.Verify(m => m.Send(It.IsAny<TaskStatusUpdatedCommand>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetAllTasks_ShouldReturnMappedTasks()
    {
        // Arrange
        var context = await CreateInMemoryDbContextAsync();
        var mapper = new Mock<IMapper>();
        var mediator = new Mock<IMediator>();

        var tasks = new List<TaskModel>
        {
            new() { Id = 1, Name = "Task 1", Description = "Description 1" },
            new() { Id = 2, Name = "Task 2", Description = "Description 2" }
        };

        // Налаштування мапінгу в мокованому mapper
        mapper.Setup(m => m.Map<IEnumerable<TaskModel>>(It.IsAny<IEnumerable<TaskEntity>>())).Returns(tasks);

        var taskService = new TaskService.TaskService(context, mapper.Object, mediator.Object);

        // Act
        var result = await taskService.GetAllTasks();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }
    
    private async Task<IAppDbContext> CreateInMemoryDbContextAsync()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        var context = new AppDbContext(options);
        await SeedDatabaseAsync(context);

        return context;
    }

    private async Task SeedDatabaseAsync(AppDbContext context)
    {
        // Додай тестові дані
        context.Tasks.Add(new TaskEntity { Id = 1, Name = "Task 1", Description = "Description 1" });
        context.Tasks.Add(new TaskEntity { Id = 2, Name = "Task 2", Description = "Description 2" });
        await context.SaveChangesAsync();
    }
}