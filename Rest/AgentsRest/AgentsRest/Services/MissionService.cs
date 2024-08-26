using AgentsRest.Data;
using AgentsRest.Dto;
using AgentsRest.Models;
using Microsoft.EntityFrameworkCore;

namespace AgentsRest.Services
{
	public class MissionService(ApplicationDbContext context) : IMissionService
	{
		public async Task<MissionModel?> GetMissionByIdAsync(int id) =>
			await context.Missions
			.Include(m => m.Agent)
			.Include(m => m.Target)
			.FirstOrDefaultAsync(m => m.Id == id);

		public async Task<IEnumerable<MissionDto>> GetAllMissionsAsync()
		{
			var missions = await context.Missions
			.Include(m => m.Agent)
			.Include(m => m.Target)
			.ToListAsync();

			return missions.Select(
				m => new MissionDto
				{
					Id = m.Id,
					AgentNickname = m.Agent.Alias,
					AgentXPosition = (int)m.Agent.LocationX,
					AgentYPosition = (int)m.Agent.LocationY,
					TargetName = m.Target.Name,
					TargetRole = m.Target.Position,
					TargetXPosition = (int)m.Target.LocationX,
					TargetYPosition = (int)m.Target.LocationY,
					Distance = CalculateDistance(
					m.Agent.LocationX,
					m.Agent.LocationY,
					m.Target.LocationX,
					m.Target.LocationY),
					Duration = TimeSpan.FromSeconds(m.ActualTime)
				});
		}


		public async Task<MissionModel> CreateMissionAsync(MissionModel model)
		{
			if (model == null)
			{ throw new Exception("Invalid mission"); }

			
			context.Missions.Add(model);
			await context.SaveChangesAsync();
			return model;
		}

		public async Task UpdateMissionStatusAsync(int id,  MissionStatus status)
		{
			var mission = await GetMissionByIdAsync(id)
				??throw new Exception($"Mission with id {id} was not found");

			mission.Status = status;
			context.Missions.Update(mission);
			await context.SaveChangesAsync();
        }


		public async Task ProposeMissionAsync()
		{
			var targets = await context.Targets
				.Where(t => t.Status == TargetStatus.Alive)
				.ToListAsync();

			var agents = await context.Agents
				.Where(a => a.Status == AgentStatus.Dormant)
				.ToListAsync();

			var missionsToPropose = agents
				.SelectMany(
				agent => targets,
				(agent, target) =>
				new {
					Agent = agent,
					Target = target,
					Distance = CalculateDistance(
					agent.LocationX,
					agent.LocationY,
					target.LocationX,
					target.LocationY) 
				})
				.Where(at => at.Distance <= 200)
				.Select(at => new MissionModel
				{
					AgentId = at.Agent.Id,
					TargetId = at.Target.Id,
					RemainingTime = CalculateRemainingTime(at.Distance),
					Status = MissionStatus.Proposed
				});
			context.Missions.AddRange(missionsToPropose);
			await context.SaveChangesAsync();
		}


		public async Task AssigenMissionAsync(int id)
		{
			var proposedMissions = await context.Missions
				.Include(m => m.Agent)
				.Include(m => m.Target)
				.Where(m => m.Status == MissionStatus.Proposed)
				.ToListAsync();
			;
		}

		public async Task ProcessMisionsAsync()
		{
			await ProposeMissionAsync();

			var missions = await context.Missions
				.Include(m => m.Agent)
				.Include(m => m.Target)
				.Where(m => m.Status == MissionStatus.Assigned || m.Status == MissionStatus.Proposed)
				.ToListAsync();

			foreach (var mission in missions)
			{
				var agentX = mission.Agent.LocationX;
				var agentY = mission.Agent.LocationY;
				var targetX = mission.Target.LocationX;
				var targetY = mission.Target.LocationY;

				var (directionX, directionY) = CakculateDirection(agentX, agentY, targetX, targetY);
				agentX += directionX;
				agentY += directionY;

				var distance = CalculateDistance(agentX, agentY, targetX, targetY);

				if (distance < 1.0)
				{
					mission.Status = MissionStatus.Completed;
					mission.Target.Status = TargetStatus.Eliminated;
					mission.Agent.Status = AgentStatus.Dormant;
				}
				else
				{
					mission.RemainingTime = CalculateRemainingTime(distance);
				}
			}
			await context.SaveChangesAsync();
		}

		private double CalculateRemainingTime(double distance) =>
			distance / 5.0;
		private double CalculateDistance(double x1, double y1, double x2, double y2) =>
			Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));

		private (double, double) CakculateDirection(double agentX, double agentY, double targetX, double targetY)
		{
			double dX = targetX - agentX;
			double dY = targetY - agentY;
			double magnitude = Math.Sqrt(dX * dX + dY * dY);
			return (dX / magnitude , dY / magnitude);
		}
	}
}
