using Domain.Core.Commands;
using Domain.Core.Events;

namespace Domain.Core.EventBus;

public interface IEventBus
{
    Task Publish<T>(T @event) where T : Event;

    Task Subscribe<T, TH>()
        where T : Event
        where TH : IEventHandler<T>;
}