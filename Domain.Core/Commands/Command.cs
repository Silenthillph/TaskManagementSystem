using Domain.Core.Events;
using MediatR;

namespace Domain.Core.Commands;

public abstract class Command : IRequest<bool>
{
    public DateTime Timestamp { get; protected set; } = DateTime.Now;
}