using AgentsRest.Data;
using AgentsRest.Dto;
using AgentsRest.Models;
using Microsoft.EntityFrameworkCore;

namespace AgentsRest.Services
{
	public class AgentService(ApplicationDbContext context) : IAgentService
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
		public async Task<AgentModel?> GetAgentByIdAsync(int id) =>
			await context.Agents.FindAsync(id);

		public async Task<IEnumerable<AgentModel>> GetAllAgentsAsync() =>
			await context.Agents.ToListAsync();

		public async Task<AgentModel> CreateAgentAsync(AgentDto agentDto)
		{
			if (agentDto == null)
			{
				throw new Exception("Invalid agent");
			}
			var agent = new AgentModel
			{
				Alias = agentDto.nickname,
				ImageUrl = agentDto.photoUrl
			};
			context.Agents.Add(agent);
			await context.SaveChangesAsync();
			return agent;
		}

		public async Task<AgentModel> PinAgentAsyinc(int id, PinAgentDto agentPin)
		{
			var existingAgent = await GetAgentByIdAsync(id)
				?? throw new Exception($"Agent by id {id} dose not exsit");
			existingAgent.LocationX = agentPin.x;
			existingAgent.LocationY = agentPin.y;
			await context.SaveChangesAsync();
			return existingAgent;
		}

		public async Task MoveAgentAsync(int id, string direction)
		{
			var agent = await GetAgentByIdAsync(id)
				?? throw new Exception($"Agent by id {id} dose not exsit");
			var currentLocationX = agent.LocationX;
			var currentLocationY = agent.LocationY;
            if (Direction.TryGetValue(direction, out var offset))
            {
				var newLocationX = agent.LocationX += offset.x;
				var newLocationY = agent.LocationY += offset.y;
				if (currentLocationX == newLocationX && currentLocationY == newLocationY)
				{ throw new Exception("Agent did not move"); }
				agent.LocationX = newLocationX;
				agent.LocationY = newLocationY;
				await context.SaveChangesAsync();
			}

		}
	}
}
