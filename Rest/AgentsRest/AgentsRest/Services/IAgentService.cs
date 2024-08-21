using AgentsRest.Models;

namespace AgentsRest.Services
{
	public interface IAgentService
	{
		Task<AgentModel?> GetAgentByIdAsync(int id);
		Task<IEnumerable<AgentModel>> GetAllAgentsAsync();
		Task<AgentModel> CreateAgent(AgentModel agent);
		Task<AgentModel> UpdateAgentStatusAsync(int id, AgentStatus status);
		Task<AgentModel> DeleteAgentAsync(int id);
	}
}
