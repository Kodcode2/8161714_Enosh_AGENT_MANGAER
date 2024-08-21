using AgentsRest.Models;
using Microsoft.EntityFrameworkCore;

namespace AgentsRest.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
			: base(options) 
		{
		
		}

		public DbSet<AgentModel> Agents { get; set; }
		public DbSet<TargetModel> Targets { get; set; }
		public DbSet<MissionModel> Missions { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<MissionModel>()
				.HasOne(m => m.Agent)
				.WithMany()
				.HasForeignKey(m => m.AgentId);
			
			modelBuilder.Entity<MissionModel>()
				.HasOne(m => m.Target)
				.WithMany()
				.HasForeignKey(m => m.TargetId);
		}
	}
}
