using AgentsRest.Dto;
using AgentsRest.Models;

namespace AgentsRest.Services
{
	public interface IAgentService
	{
		Task<AgentModel?> GetAgentByIdAsync(int id);
		Task<IEnumerable<AgentModel>> GetAllAgentsAsync();
		Task<AgentModel> CreateAgentAsync(AgentDto agent);
		Task<AgentModel> PinAgentAsyinc(int id, PinAgentDto agentPin);
		Task MoveAgentAsync(int id, string direction);
	}
}
