namespace AgentsRest.Models
{
	public class AgentModel
	{
		public int Id { get; set; }
		public string Alias { get; set; }
		public string ImageUrl { get; set; }
		public double LocationX { get; set; }
		public double LocationY { get; set; }
		public AgentStatus Status { get; set; }
	}

	public enum AgentStatus
	{
		Dormant,
		Active
	}
}
