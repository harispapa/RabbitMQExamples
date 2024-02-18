using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Banking.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class BankingController(ILogger<BankingController> logger, IAccountService accountService) : ControllerBase
	{
		[HttpGet]
		public ActionResult<List<Account>> Get() =>
			Ok(accountService.GetAccounts());
	}
}
