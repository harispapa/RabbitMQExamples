using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using MicroRabbit.Banking.Infrastructure.Persistence.Context;

namespace MicroRabbit.Banking.Infrastructure.Persistence.Repository;

public class AccountRepository(BankingDbContext dbContext) : IAccountRepository
{
	public IEnumerable<Account> GetAccounts() => 
		dbContext.Accounts.ToList();
}