using AgentsRest.Models;
using AgentsRest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgentsRest.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class missionsController(IMissionService missionService) : ControllerBase
	{
		[HttpPost("update")]
		public async Task<IActionResult> ProcessMissions()
		{
			try
			{
				await missionService.ProcessMisionsAsync();
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpGet]
		public async Task<IActionResult> GetAllMissions()
		{
			var missions = await missionService.GetAllMissionsAsync();
			return Ok(missions);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateMission(int id, [FromBody] MissionStatus status)
		{
			try
			{
				await missionService.UpdateMissionStatusAsync(id, status);
				return NoContent();
			}
			catch (Exception ex)
			{ return NotFound(ex.Message); }
		}

	}
}
