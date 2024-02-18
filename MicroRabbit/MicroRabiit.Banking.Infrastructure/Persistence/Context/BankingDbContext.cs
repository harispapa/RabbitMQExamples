using MicroRabbit.Banking.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroRabbit.Banking.Infrastructure.Persistence.Context;

public class BankingDbContext : DbContext
{
	public BankingDbContext(DbContextOptions<BankingDbContext> options) : base(options)
	{
	}

	public DbSet<Account> Accounts { get; set; }
};