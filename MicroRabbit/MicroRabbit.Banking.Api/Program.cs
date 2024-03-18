using System.Reflection;
using MicroRabbit.Banking.Infrastructure.Persistence.Context;
using MicroRabbit.Infrastructure.IoC;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

namespace MicroRabbit.Banking.Api;

public class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateBuilder(args);

		builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

		// Add services to the container.
		DependencyContainer.RegisterServices(builder.Services, ApiProject.Banking);

		builder.Services.AddDbContext<BankingDbContext>(options =>
		{
			var connectionString = builder.Configuration.GetConnectionString("BankingDbConnection");

			options.UseSqlServer(connectionString);
		});

		builder.Services.AddControllers();

		// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddSwaggerGen(c =>
		{
			c.SwaggerDoc("v1", new OpenApiInfo() { Title = "banking Microservice", Version = "v1"});
		});

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (app.Environment.IsDevelopment())
		{
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Banking Microservice V1");
			});
		}

		app.UseHttpsRedirection();

		app.UseAuthorization();


		app.MapControllers();

		app.Run();
	}
}