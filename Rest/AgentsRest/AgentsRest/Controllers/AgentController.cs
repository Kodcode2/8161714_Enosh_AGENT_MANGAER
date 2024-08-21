using AgentsRest.Models;
using AgentsRest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgentsRest.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AgentController(IAgentService agentService) : ControllerBase
	{
		[HttpGet("{id}")]
		public async Task<IActionResult> GetAgentById(int id)
		{
			var agent = await agentService.GetAgentByIdAsync(id);
			if (agent == null) 
				return NotFound();
			return Ok(agent);
		}

		[HttpGet]
		public async Task<IActionResult> GetAllAgents()
		{
			var agents = await agentService.GetAllAgentsAsync();
			return Ok(agents);
		}

		[HttpPost]
		public async Task<IActionResult> CreateAgent([FromBody] AgentModel model)
		{
			try
			{
				var agent = await agentService.CreateAgent(model);
				return Created("", agent);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPut("{id}/status")]
		public async Task<IActionResult> UpdateAgentStatus(int id, [FromBody] AgentModel model)
		{
			try
			{
				var updated = await agentService.UpdateAgentStatusAsync(id, model.Status);
				return Ok(updated);
			}
			catch (Exception ex)
			{
				return NotFound(ex.Message);
			}
		}

		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> DeleteAgent(int id)
		{ 
			try
			{
				var deleted = await agentService.DeleteAgentAsync(id);
				return Ok(deleted);
			}
			catch (Exception ex)
			{
				return NotFound(ex.Message);
			}
		}
	}
}
