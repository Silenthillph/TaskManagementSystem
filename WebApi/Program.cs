using System.Reflection;
using Domain.Core.EventBus;
using Domain.Infrastructure;
using Domain.Infrastructure.Context;
using MessagingService;
using Microsoft.EntityFrameworkCore;
using TaskService;
using TaskService.EventHandlers;
using TaskService.Events;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

// Add Infrastructure
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("DefaultConnection"));

// Add Task Service
builder.Services.AddRabbitMQEventBus();
builder.Services.AddTaskService();

var app = builder.Build();

// create and update db
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

// subscribe to status change action from another services
var eventBus = app.Services.GetRequiredService<IEventBus>();
await eventBus.Subscribe<TaskStatusChangeActionEvent, TaskStatusChangeActionEventHandler>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();