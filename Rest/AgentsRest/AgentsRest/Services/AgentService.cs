using AgentsRest.Data;
using AgentsRest.Models;
using Microsoft.EntityFrameworkCore;

namespace AgentsRest.Services
{
	public class AgentService(ApplicationDbContext context) : IAgentService
	{
		public async Task<AgentModel?> GetAgentByIdAsync(int id) =>
			await context.Agents.FindAsync(id);

		public async Task<IEnumerable<AgentModel>> GetAllAgentsAsync() =>
			await context.Agents.ToListAsync();

		public async Task<AgentModel> CreateAgent(AgentModel agent)
		{
			if (agent == null)
			{ 
				throw new ArgumentNullException("Invalid agent");
			}
			context.Agents.Add(agent);
			await context.SaveChangesAsync();
			return agent;
		}

		public async Task<AgentModel> UpdateAgentStatusAsync(int id, AgentStatus status)
		{
			var agent = await context.Agents.FindAsync(id);
			if (agent == null) { throw new ArgumentNullException("Agent not found"); }

			if(status == AgentStatus.Dormant && context.Missions
				.Any(m => m.AgentId == id && m.Status == MissionStatus.Assigned))
			{
				throw new InvalidOperationException("Can't cancel assingment befour eliminating target");
			}

			agent.Status = status;
			await context.SaveChangesAsync();
			return agent;
		}

		public async Task<AgentModel> DeleteAgentAsync(int id)
		{
			var agent = await context.Agents.FindAsync(id);
			if (agent == null) { throw new ArgumentNullException("Agent id not found"); }
			
			context.Agents.Remove(agent);
			await context.SaveChangesAsync();
			return agent;
		}
	}
}
