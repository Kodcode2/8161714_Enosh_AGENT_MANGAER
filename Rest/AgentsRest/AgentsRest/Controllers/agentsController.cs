using AgentsRest.Dto;
using AgentsRest.Models;
using AgentsRest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AgentsRest.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class agentsController(IAgentService agentService) : ControllerBase
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
		public async Task<IActionResult> CreateAgent([FromBody] AgentDto dto)
		{
			try
			{
				var agent = await agentService.CreateAgentAsync(dto);
				return Created("", agent);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPut("{id}/pin")]
		public async Task<IActionResult> PinAgent(int id, [FromBody] PinAgentDto agentPin)
		{
			try
			{
				var updated = await agentService.PinAgentAsyinc(id, agentPin);
				return Ok(updated);
			}
			catch (Exception ex)
			{
				return NotFound(ex.Message);
			}
		}


		[HttpPut("{id}/move")]
		public async Task<IActionResult> MoveAgent(int id, [FromBody] MoveAgentDto direction)
		{
			try
			{ 
				await agentService.MoveAgentAsync(id, direction.direction);
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
