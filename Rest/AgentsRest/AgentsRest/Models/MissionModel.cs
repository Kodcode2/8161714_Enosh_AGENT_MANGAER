namespace AgentsRest.Models
{
	public class MissionModel
	{
		public int Id { get; set; }
		public int AgentId { get; set; }
		public AgentModel Agent { get; set; }
		public int TargetId { get; set; }
		public TargetModel Target { get; set; }
		public double RemainingTime { get; set; }
		public double ActualTime { get; set; }
		public MissionStatus Status { get; set; }
	}

	public enum MissionStatus
	{
		Proposed,
		Assigned,
		Completed
	}
}
