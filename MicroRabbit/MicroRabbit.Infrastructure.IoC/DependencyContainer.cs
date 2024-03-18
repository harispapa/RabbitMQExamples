using MediatR;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Domain.CommandHandlers;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Infrastructure.Persistence.Context;
using MicroRabbit.Banking.Infrastructure.Persistence.Repository;
using MicroRabbit.Domain.Core.Bus.Interfaces;
using MicroRabbit.Infrastructure.Bus;
using MicroRabbit.Transfer.Application.Interfaces;
using MicroRabbit.Transfer.Application.Services;
using MicroRabbit.Transfer.Domain.EventJHandlers;
using MicroRabbit.Transfer.Domain.Events;
using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Infrastructure.Persistence.Context;
using MicroRabbit.Transfer.Infrastructure.Persistence.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace MicroRabbit.Infrastructure.IoC;

public enum ApiProject
{
	Banking,
	Transfer
}

public class DependencyContainer
{
	public static void RegisterServices(IServiceCollection services, ApiProject project )
	{
		// Domain Bus
		services.AddTransient<IEventBus, RabbitMqBus>();

		if (project == ApiProject.Banking)
		{
			//Domain Banking Commands
			services.AddTransient<IRequestHandler<CreateTransferCommand, bool>, TransferCommandHandler>();

			// Application Layer
			services.AddTransient<IAccountService, AccountService>();

			// Infrastructure - Data
			services.AddTransient<IAccountRepository, AccountRepository>();
			services.AddTransient<BankingDbContext>();
		}

		if (project == ApiProject.Transfer)
		{
			//Domain Banking Commands
			services.AddTransient<IEventHandler<TransferCreatedEvent>, TransferEventHandler>();

			// Application Layer
			services.AddTransient<ITransferService, TransferService>();

			// Infrastructure - Data
			services.AddTransient<ITransferRepository, TransferRepository>();
			services.AddTransient<TransferDbContext>();
		}
	}
}