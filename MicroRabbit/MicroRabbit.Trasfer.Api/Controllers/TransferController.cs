using MicroRabbit.Transfer.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Transfer.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class TransferController(ITransferService transferService) : ControllerBase
	{
		[HttpGet]
		public IActionResult Get() =>
			Ok(transferService.GetTransferLogs());
	}
}
