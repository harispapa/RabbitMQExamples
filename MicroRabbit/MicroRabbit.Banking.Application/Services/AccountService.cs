using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using MicroRabbit.Domain.Core.Bus.Interfaces;

namespace MicroRabbit.Banking.Application.Services;

public class AccountService(IAccountRepository accountRepository, IEventBus eventbus) : IAccountService
{
	public IEnumerable<Account> GetAccounts() =>
		accountRepository.GetAccounts();

	public void Transfer(AccountTransfer accountTransfer)
	{
		var createTransferCommand = new CreateTransferCommand(accountTransfer.FromAccount, accountTransfer.ToAccount, accountTransfer.TransferAmount);

		eventbus.SendCommand(createTransferCommand);
	}
}