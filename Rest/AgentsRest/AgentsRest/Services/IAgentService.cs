using AgentsRest.Models;

namespace AgentsRest.Services
{
	public interface IAgentService
	{
		Task<AgentModel?> GetAgentByIdAsync(int id);
		Task<IEnumerable<AgentModel>> GetAllAgentsAsync();
		Task<AgentModel> CreateAgent(AgentModel agent);
		void UpdateAgentAsync(int id, AgentModel agent);
		void MoveAgent(int id, Direction direction);
	}
}
