using Domain.Core.Events;

namespace Domain.Core.EventBus;

public interface IEventHandler<in TEvent> where TEvent : Event
{
    Task Handle(TEvent @event);
}

