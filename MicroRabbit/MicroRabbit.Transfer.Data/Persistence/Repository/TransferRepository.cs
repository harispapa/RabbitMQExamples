using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Domain.Models;
using MicroRabbit.Transfer.Infrastructure.Persistence.Context;

namespace MicroRabbit.Transfer.Infrastructure.Persistence.Repository;

public class TransferRepository(TransferDbContext dbContext) : ITransferRepository
{
	public IEnumerable<TransferLog> GetTransferLogs() =>
		dbContext.TransferLogs
			.ToList();

	public void Add(TransferLog transferLog)
	{
		dbContext.Add(transferLog);
		dbContext.SaveChanges();
	}
}