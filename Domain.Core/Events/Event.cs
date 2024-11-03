namespace Domain.Core.Events;
public abstract class Event
{
    public DateTime Timestamp { get; protected set; } = DateTime.Now;
}