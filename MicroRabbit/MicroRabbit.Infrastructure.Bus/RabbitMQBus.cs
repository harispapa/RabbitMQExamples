using MediatR;
using MicroRabbit.Domain.Core.Bus.Interfaces;
using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;

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
			Uri = new Uri($"amqp://admin:admin@localhost:5672/")
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
		var eventName = typeof(T).Name;
		var handlerType = typeof(THandler);

		if (!_eventTypes.Contains(typeof(T))) 
			_eventTypes.Add(typeof(T));

		if (!_handlers.ContainsKey(eventName)) 
			_handlers.Add(eventName, []);

		if (_handlers[eventName].Any(s => s == handlerType))
			throw new ArgumentException($"Handler Type {handlerType.Name} already is register for {eventName}", nameof(handlerType));

		_handlers[eventName].Add(handlerType);

		StartBasicConsume<T>();
	}

	private void StartBasicConsume<T>() where T : Event
	{
		var factory = new ConnectionFactory()
		{
			Uri = new Uri($"amqp://admin:admin@localhost:5672/"),
			DispatchConsumersAsync = true
		};

		var con = factory.CreateConnection();
		var channel = con.CreateModel();

		var eventName = typeof(T).Name;

		channel.QueueDeclare(eventName, false, false, false, null);

		var consumer = new AsyncEventingBasicConsumer(channel);
		consumer.Received += Consumer_Received;

		channel.BasicConsume(eventName, true, consumer);
	}

	private async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
	{

	}
}