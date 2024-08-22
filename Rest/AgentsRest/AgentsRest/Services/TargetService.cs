using AgentsRest.Data;
using AgentsRest.Dto;
using AgentsRest.Models;
using Microsoft.EntityFrameworkCore;

namespace AgentsRest.Services
{
	public class TargetService(ApplicationDbContext context) : ITargetService
	{
		private readonly Dictionary<string, (double x, double y)> Direction = new()
		{
		  {"n", (x: 0, y: 1) },
		  {"s", (x: 0, y: -1) },
		  {"e", (x: 1, y: 0) },
		  {"w", (x: -1, y: 0) },
		  {"nw", (x: -1, y: 1) },
		  {"ne", (x: 1, y: 1) },
		  {"sw", (x: -1, y: -1) },
		  {"se", (x: 1, y: -1) }
		};
		public async Task<TargetModel?> GetTargetByIdAsync(int id) =>
			await context.Targets.FindAsync(id);

		public async Task<IEnumerable<TargetModel>> GetAllTargetsAsync() =>
			await context.Targets.ToListAsync();

		public async Task<TargetModel> CreateTargetAsync(TargetDto targetDto)
		{
			if (targetDto == null)
			{
				throw new Exception("Invalid target");
			}
			var target = new TargetModel
			{
				Name = targetDto.name,
				Position = targetDto.position,
				ImageUrl = targetDto.photoUrl
			};
			context.Targets.Add(target);
			await context.SaveChangesAsync();
			return target;
		}

		public async Task<TargetModel> PinTargetAsyinc(int id, PinTargetDto targetPin)
		{
			var existingTarget = await GetTargetByIdAsync(id)
				?? throw new Exception($"Target by id {id} dose not exsit");
			existingTarget.LocationX = targetPin.x;
			existingTarget.LocationY = targetPin.y;
			await context.SaveChangesAsync();
			return existingTarget;
		}

		public async Task MoveTargetAsync(int id, string direction)
		{
			var target = await GetTargetByIdAsync(id)
				?? throw new Exception($"Target by id {id} dose not exsit");
			var currentLocationX = target.LocationX;
			var currentLocationY = target.LocationY;
			if (Direction.TryGetValue(direction, out var offset))
			{
				var newLocationX = target.LocationX += offset.x;
				var newLocationY = target.LocationY += offset.y;
				if (currentLocationX == newLocationX && currentLocationY == newLocationY)
				{ throw new Exception("Target did not move"); }
				target.LocationX = newLocationX;
				target.LocationY = newLocationY;
				await context.SaveChangesAsync();
			}

		}
	}
}
