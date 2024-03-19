using MicroRabbit.Domain.Core.Bus.Interfaces;
using MicroRabbit.Transfer.Domain.Events;
using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Domain.Models;

namespace MicroRabbit.Transfer.Domain.EventJHandlers;

public class TransferEventHandler(ITransferRepository transferRepository) : IEventHandler<TransferCreatedEvent>
{
	public Task Handle(TransferCreatedEvent @event)
	{
		transferRepository.Add(new TransferLog()
		{
			FromAccount = @event.From,
			ToAccount = @event.To,
			TransferAmount = @event.Amount
		});

		return Task.CompletedTask;
	}
}