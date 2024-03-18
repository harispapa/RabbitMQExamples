using MicroRabbit.Transfer.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroRabbit.Transfer.Infrastructure.Persistence.Context;

public class TransferDbContext : DbContext
{
	public TransferDbContext(DbContextOptions<TransferDbContext> options) : base(options)
	{
	}

	public DbSet<TransferLog> TransferLogs { get; set; }
};