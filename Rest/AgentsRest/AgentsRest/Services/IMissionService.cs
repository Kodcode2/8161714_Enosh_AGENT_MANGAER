using AgentsRest.Dto;
using AgentsRest.Models;

namespace AgentsRest.Services
{
	public interface IMissionService
	{
		Task<MissionModel?> GetMissionByIdAsync(int id);
		Task<IEnumerable<MissionDto>> GetAllMissionsAsync();
		Task<MissionModel> CreateMissionAsync(MissionModel model);
		Task UpdateMissionStatusAsync(int id, MissionStatus status);
		Task ProposeMissionAsync();
		Task AssigenMissionAsync(int missionId);
		Task ProcessMisionsAsync();
	}
}
