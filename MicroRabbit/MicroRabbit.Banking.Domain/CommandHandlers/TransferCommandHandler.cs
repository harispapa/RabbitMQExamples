using MediatR;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Events;
using MicroRabbit.Domain.Core.Bus.Interfaces;

namespace MicroRabbit.Banking.Domain.CommandHandlers;

public class TransferCommandHandler(IEventBus eventBus) : IRequestHandler<CreateTransferCommand, bool>
{
	public Task<bool> Handle(CreateTransferCommand request, CancellationToken cancellationToken)
	{
		// Publish Event to Bus
		eventBus.Publish(new TransferCreatedEvent(request.From, request.To, request.Amount));

		return Task.FromResult(true);
	}
}