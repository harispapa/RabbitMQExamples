using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;

namespace MicroRabbit.Banking.Application.Services;

public class AccountService(IAccountRepository accountRepository) : IAccountService
{
	public IEnumerable<Account> GetAccounts() => 
		accountRepository.GetAccounts();
}