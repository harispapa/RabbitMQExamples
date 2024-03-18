using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Infrastructure.Persistence.Context;
using MicroRabbit.Banking.Infrastructure.Persistence.Repository;
using MicroRabbit.Domain.Core.Bus.Interfaces;
using MicroRabbit.Infrastructure.Bus;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using MediatR;
using MicroRabbit.Banking.Domain.CommandHandlers;
using MicroRabbit.Banking.Domain.Commands;

namespace MicroRabbit.Infrastructure.IoC;

public class DependencyContainer
{
	public static void RegisterServices(IServiceCollection services)
	{
		// Domain Bus
		services.AddTransient<IEventBus, RabbitMqBus>();

		//Domain Banking Commands
		services.AddTransient<IRequestHandler<CreateTransferCommand, bool>, TransferCommandHandler>();

		// Application Layer
		services.AddTransient<IAccountService, AccountService>();

		// Infrastructure - Data
		services.AddTransient<IAccountRepository, AccountRepository>();
		services.AddTransient<BankingDbContext>();
	}
}