using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Infrastructure.Persistence.Context;
using MicroRabbit.Banking.Infrastructure.Persistence.Repository;
using MicroRabbit.Domain.Core.Bus.Interfaces;
using MicroRabbit.Infrastructure.Bus;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MicroRabbit.Infrastructure.IoC;

public class DependencyContainer
{
	public static void RegisterServices(IServiceCollection services)
	{
		// Domain Bus
		services.AddTransient<IEventBus, RabbitMqBus>();

		// Application Layer
		services.AddTransient<IAccountService, AccountService>();

		// Infrastructure 
		services.AddTransient<IAccountRepository, AccountRepository>();
		services.AddTransient<BankingDbContext>();

		services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
	}
}