using System.Text.Json;
using ClientAgent.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ClientAgent.Controllers
{
	public class MissionController(IHttpClientFactory clientFactory) : Controller
	{
		private readonly string baseUrl = "https://localhost:7244/missions";

		public async Task<IActionResult> Index()
		{
			var httpClient = clientFactory.CreateClient();
			var result = await httpClient.GetAsync(baseUrl);
			if (result.IsSuccessStatusCode)
			{ 
				var content = await result.Content.ReadAsStringAsync();
				var missions = JsonSerializer.Deserialize<IEnumerable<MissionVM>>(content);
				return View(missions);
			}
				return RedirectToAction("Index", "Home");
		}
		
		public async Task<IActionResult> Details(int id)
		{
			var httpClient = clientFactory.CreateClient();
			var result = await httpClient.GetAsync($"{baseUrl}/{id}");
			if (result.IsSuccessStatusCode)
			{ 
				var content = await result.Content.ReadAsStringAsync();
				var mission = JsonSerializer.Deserialize<MissionVM>(content);
				return View(mission);
			}
				return RedirectToAction("Index", "Home");
		}
		
		public async Task<IActionResult> Assign(int id)
		{
			var httpClient = clientFactory.CreateClient();
			var result = await httpClient.PutAsync($"{baseUrl}/{id}", null);
			result.EnsureSuccessStatusCode();
			return RedirectToAction("Index", "Home");
		}


	}
}
