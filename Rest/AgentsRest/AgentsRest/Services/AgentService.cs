using AgentsRest.Data;
using AgentsRest.Models;
using Microsoft.EntityFrameworkCore;

namespace AgentsRest.Services
{
	public class AgentService(ApplicationDbContext context) : IAgentService
	{
		private readonly Dictionary<Direction, Action<AgentModel>> directionMovment = new Dictionary<Direction, Action<AgentModel>> 
		{
			{Direction.North, agent => agent.LocationY += 1 },
			{Direction.South, agent => agent.LocationY -= 1 },
			{Direction.East, agent => agent.LocationX += 1 },
			{Direction.West, agent => agent.LocationX -= 1 },
			{Direction.Northeast, agent => {agent.LocationX += 1; agent.LocationY += 1; } },
			{Direction.NorthWest, agent => {agent.LocationX -= 1; agent.LocationY += 1; } },
			{Direction.Southeast, agent => {agent.LocationX += 1; agent.LocationY -= 1; } },
			{Direction.Southwest, agent => {agent.LocationX -= 1; agent.LocationY -= 1; } },
		};
		public async Task<AgentModel?> GetAgentByIdAsync(int id) =>
			await context.Agents.FindAsync(id);

		public async Task<IEnumerable<AgentModel>> GetAllAgentsAsync() =>
			await context.Agents.ToListAsync();

		public async Task<AgentModel> CreateAgent(AgentModel agent)
		{
			if (agent == null)
			{ 
				throw new Exception("Invalid agent");
			}
			context.Agents.Add(agent);
			await context.SaveChangesAsync();
			return agent;
		}

		public void UpdateAgentAsync(int id, AgentModel agent)
		{
			var existingAgent = context.Agents.Find(id)
				?? throw new Exception($"Agent by id {id} dose not exsit");
			existingAgent.Alias = agent.Alias;
			existingAgent.ImageUrl = agent.ImageUrl;
			existingAgent.LocationX = agent.LocationX;
			existingAgent.LocationY = agent.LocationY;
			existingAgent.Status = agent.Status;
			context.SaveChangesAsync();
		}

		public void MoveAgent(int id, Direction direction)
		{
			var agent = context.Agents.Find(id)
				?? throw new Exception($"Agent by id {id} dose not exsit");
			directionMovment[direction](agent);
			context.SaveChanges();
		}
	}
}
