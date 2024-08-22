using AgentsRest.Dto;
using AgentsRest.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace AgentsRest.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class targetsController(ITargetService targetService) : ControllerBase
	{
		[HttpGet("{id}")]
		public async Task<IActionResult> GetTargetById(int id)
		{
			var target = await targetService.GetTargetByIdAsync(id);
			if (target == null)
				return NotFound();
			return Ok(target);
		}

		[HttpGet]
		public async Task<IActionResult> GetAllTargets()
		{
			var targets = await targetService.GetAllTargetsAsync();
			return Ok(targets);
		}

		[HttpPost]
		public async Task<IActionResult> CreateTarget([FromBody] TargetDto dto)
		{
			try
			{
				var target = await targetService.CreateTargetAsync(dto);
				return Created("", target);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		[HttpPut("{id}/pin")]
		public async Task<IActionResult> PinTarget(int id, [FromBody] PinTargetDto targetPin)
		{
			try
			{
				var updated = await targetService.PinTargetAsyinc(id, targetPin);
				return Ok(updated);
			}
			catch (Exception ex)
			{
				return NotFound(ex.Message);
			}
		}


		[HttpPut("{id}/move")]
		public async Task<IActionResult> MoveTarget(int id, [FromBody] MoveTargetDto direction)
		{
			try
			{
				await targetService.MoveTargetAsync(id, direction.direction);
				return Ok();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}
	}
}
