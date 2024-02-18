using MicroRabbit.Banking.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroRabbit.Banking.Infrastructure.Persistence.Context;

public class BankingDbContext(DbContextOptions options) : DbContext(options)
{
	public DbSet<Account> Accounts { get; set; }
};