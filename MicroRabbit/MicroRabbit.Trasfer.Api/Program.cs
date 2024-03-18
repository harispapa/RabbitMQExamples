using System.Reflection;
using MicroRabbit.Domain.Core.Bus.Interfaces;
using MicroRabbit.Infrastructure.IoC;
using MicroRabbit.Transfer.Domain.EventJHandlers;
using MicroRabbit.Transfer.Domain.Events;
using MicroRabbit.Transfer.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace MicroRabbit.Transfer.Api;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

		// Add services to the container.
		DependencyContainer.RegisterServices(builder.Services, ApiProject.Transfer);

		builder.Services.AddDbContext<TransferDbContext>(options =>
		{
			var connectionString = builder.Configuration.GetConnectionString("TransferDbConnection");

			options.UseSqlServer(connectionString);
		});

		builder.Services.AddControllers();
		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen();

		var app = builder.Build();

		var eventBus = ((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IEventBus>();
		eventBus.Subscribe<TransferCreatedEvent, TransferEventHandler>();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI();
		}

		app.UseHttpsRedirection();

		app.UseAuthorization();


		app.MapControllers();

		app.Run();
	}
}