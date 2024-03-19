using MicroRabbit.MVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MicroRabbit.MVC.Models.Dto;
using MicroRabbit.MVC.Services;

namespace MicroRabbit.MVC.Controllers
{
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly ITransferService _transferService;

		public HomeController(ILogger<HomeController> logger, ITransferService transferService)
		{
			_logger = logger;
			_transferService = transferService;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[HttpPost]
		public async Task<IActionResult> Transfer(TransferViewModel transferViewModel)
		{
			var transDto = new TransferDto()
			{
				FromAccount = transferViewModel.FromAccount,
				ToAccount = transferViewModel.ToAccount,
				TransferAmount = transferViewModel.TransferAmount
			};

			await _transferService.Transfer(transDto);

			return View("Index");
		}
	}
}
