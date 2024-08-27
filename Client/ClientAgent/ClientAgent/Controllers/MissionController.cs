using System.Text.Json;
using ClientAgent.Services;
using ClientAgent.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace ClientAgent.Controllers
{
	public class MissionController(MissionService missionService) : Controller
	{
        public async Task<IActionResult> Index()
        {
            var missions = await missionService.GetAllMissionsAsync();
            return View(missions);
        }

        public async Task<IActionResult> Details(int id)
        {
            var mission = await missionService.GetMissionByIdAsync(id);
            return View(mission);
        }

        public async Task<IActionResult> Assign(int id)
        {
            await missionService.AssignMissionAsync(id);
            return RedirectToAction("Index");
        }
    }
}
