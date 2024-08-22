using AgentsRest.Dto;
using AgentsRest.Models;

namespace AgentsRest.Services
{
	public interface ITargetService
	{
		Task<TargetModel?> GetTargetByIdAsync(int id);
		Task<IEnumerable<TargetModel>> GetAllTargetsAsync();
		Task<TargetModel> CreateTargetAsync(TargetDto target);
		Task<TargetModel> PinTargetAsyinc(int id, PinTargetDto targetPin);
		Task MoveTargetAsync(int id, string direction);
	}
}
