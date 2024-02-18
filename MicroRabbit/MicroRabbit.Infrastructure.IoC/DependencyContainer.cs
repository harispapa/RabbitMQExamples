using MicroRabbit.Domain.Core.Bus.Interfaces;
using MicroRabbit.Infrastructure.Bus;
using Microsoft.Extensions.DependencyInjection;

namespace MicroRabbit.Infrastructure.IoC;

public class DependencyContainer
{
	public static void RegisterServices(IServiceCollection services)
	{
		// Domain Bus
		services.AddTransient<IEventBus, RabbitMqBus>();
	}
}