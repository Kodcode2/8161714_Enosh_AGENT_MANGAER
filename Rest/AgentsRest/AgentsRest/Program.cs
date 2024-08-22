using AgentsRest.Data;
using AgentsRest.Services;
using Microsoft.EntityFrameworkCore;

namespace AgentsRest
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddControllers();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddDbContext<ApplicationDbContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

			// Register your scoped service before building the app
			builder.Services.AddScoped<IAgentService, AgentService>();
			builder.Services.AddScoped<ITargetService, TargetService>();
			builder.Services.AddScoped<IMissionService, MissionService>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();
			app.UseAuthorization();
			app.MapControllers();
			app.Run();
		}
	}
}
