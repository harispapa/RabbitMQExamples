using MicroRabbit.MVC.Models.Dto;
using Newtonsoft.Json;

namespace MicroRabbit.MVC.Services;

public class TransferService : ITransferService
{
	private readonly HttpClient _httpClient;

	public TransferService(HttpClient httpClient) =>
		_httpClient = httpClient;

	public async Task Transfer(TransferDto transferDto)
	{
		var uri = "https://localhost:7130/Banking";
		var transferContent = new StringContent(JsonConvert.SerializeObject(transferDto), System.Text.Encoding.UTF8,
			"application/json");

		var response = await _httpClient.PostAsync(uri, transferContent);
		response.EnsureSuccessStatusCode();
	}
}