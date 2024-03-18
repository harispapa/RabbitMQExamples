using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Trasfer.Api.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class WeatherForecastController : ControllerBase
	{
		[HttpGet]
		public IActionResult Get() => 
			Ok("Hello");
	}
}
