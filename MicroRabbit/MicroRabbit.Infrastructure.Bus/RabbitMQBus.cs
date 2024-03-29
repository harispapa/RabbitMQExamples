﻿using MediatR;
using MicroRabbit.Domain.Core.Bus.Interfaces;
using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace MicroRabbit.Infrastructure.Bus;

public sealed class RabbitMqBus(IMediator mediator, IServiceScopeFactory scopeFactory) : IEventBus
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
		var eventName = @event.RoutingKey;
		var message = Encoding.UTF8.GetString(@event.Body.ToArray());

		try
		{
			await ProcessEvent(eventName, message).ConfigureAwait(false);
		}
		catch (Exception)
		{
			// ignored
		}
	}

	private async Task ProcessEvent(string eventName, string message)
	{
		if (_handlers.ContainsKey(eventName))
		{
			using (var scope = scopeFactory.CreateScope())
			{
				var subscriptions = _handlers[eventName];

				foreach (var subscription in subscriptions)
				{
					var handler = scope.ServiceProvider.GetService(subscription);
					if (handler is null)
						continue;

					var eventType = _eventTypes.SingleOrDefault(t => t.Name == eventName);
					var @event = JsonConvert.DeserializeObject(message, eventType!);
					var conreteType = typeof(IEventHandler<>).MakeGenericType(eventType!);

					await ((Task)conreteType.GetMethod("Handle")?.Invoke(handler, [@event])!);
				}
			}
		}


			
	}
}