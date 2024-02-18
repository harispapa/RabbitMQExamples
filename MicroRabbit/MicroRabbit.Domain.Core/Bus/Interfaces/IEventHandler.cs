using MicroRabbit.Domain.Core.Events;

namespace MicroRabbit.Domain.Core.Bus.Interfaces;

public interface IEventHandler<in TEvent> : IEventHandler
	where TEvent : Event
{
	Task Handle(TEvent @event);
}

public interface IEventHandler
{
}