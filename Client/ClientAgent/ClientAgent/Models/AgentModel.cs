namespace ClientAgent.Models
{
	public class AgentModel
	{
		public int Id { get; set; }
		public string Niknameame { get; set; }
		public AgentStatus Status { get; set; }
		public int LocationX { get; set; }
		public int LocationY { get; set; }



		public enum AgentStatus
		{
			Dormant,
			Active
		}
	}
}
