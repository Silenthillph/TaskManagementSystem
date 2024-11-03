using Domain.Core.Events;

namespace TaskEventsTracker.Events;

public class TaskCreateEvent(int taskId, string taskName, string taskDescription) : Event
{
    public int TaskId { get; } = taskId;
    public string TaskName { get; } = taskName;
    public string TaskDescription { get; } = taskDescription;
}