using MicroRabbit.Transfer.Domain.Models;

namespace MicroRabbit.Transfer.Domain.Interfaces;

public interface ITransferRepository
{
	public IEnumerable<TransferLog> GetTransferLogs();
}