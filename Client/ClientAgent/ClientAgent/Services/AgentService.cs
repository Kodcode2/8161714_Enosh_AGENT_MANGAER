using System.Text.Json;
using ClientAgent.Models;

namespace ClientAgent.Services
{
    public class AgentService(HttpClient httpClient)
    {
        private readonly string baseUrl = "https://localhost:7244/agents";

        public async Task<IEnumerable<AgentModel>> GetAllAgentsAsync()
        {
            var response = await httpClient.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var agents = JsonSerializer.Deserialize<IEnumerable<AgentModel>>(
                content,
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
                );
            return agents;
        }


        public async Task<AgentModel> GetAgentByIdAsync(int id)
        {
            var response = await httpClient.GetAsync($"{baseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var agent = JsonSerializer.Deserialize<AgentModel>(
                content,
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
                );
            return agent;
        }
    }
}
