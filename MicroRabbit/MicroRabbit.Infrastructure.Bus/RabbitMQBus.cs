using MediatR;
using MicroRabbit.Domain.Core.Bus.Interfaces;
using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace MicroRabbit.Infrastructure.Bus;

public sealed class RabbitMqBus(IMediator mediator) : IEventBus
{
	private readonly Dictionary<string, List<Type>> _handlers = new();
	private readonly List<Type> _eventTypes = new();

	public Task SendCommand<T>(T command) where T : Command => 
		mediator.Send(command);

	public void Publish<T>(T @event) where T : Event
	{
		var factory = new ConnectionFactory()
		{
			Uri = new Uri("amqp://admin:admin@localhost:5672/")
		};

		using var con = factory.CreateConnection();
		using var channel = con.CreateModel();

		var eventName = @event.GetType().Name;

		channel.QueueDeclare(eventName, false, false, false, null);

		var message = JsonConvert.SerializeObject(@event);
		var body = Encoding.UTF8.GetBytes(message);

		channel.BasicPublish("", eventName, null, body);
	}

	public void Subscribe<T, THandler>() where T : Event where THandler : IEventHandler<T>
	{
		throw new NotImplementedException();
	}
}