using MicroRabbit.Domain.Core.Bus.Interfaces;
using MicroRabbit.Transfer.Domain.Events;

namespace MicroRabbit.Transfer.Domain.EventJHandlers;

public class TransferEventHandler : IEventHandler<TransferCreatedEvent>
{
	public TransferEventHandler()
	{
	}

	public Task Handle(TransferCreatedEvent @event)
	{
		return Task.CompletedTask;
	}
}