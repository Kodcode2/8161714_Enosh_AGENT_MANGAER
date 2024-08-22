namespace AgentsRest.Models
{
	public class TargetModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Position { get; set; }
		public string ImageUrl { get; set; }
		public double LocationX { get; set; }
		public double LocationY { get; set; }
		public TargetStatus Status { get; set; }
	}

	public enum TargetStatus
	{
		Alive,
		Eliminated
	}
}
