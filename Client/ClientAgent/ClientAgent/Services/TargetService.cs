using ClientAgent.Models;
using System.Text.Json;

namespace ClientAgent.Services
{
    public class TargetService(HttpClient httpClient)
    {
        private readonly string baseUrl = "https://localhost:7244/targets";

        public async Task<IEnumerable<TargetModel>> GetAllTargetsAsync()
        {
            var response = await httpClient.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var targets = JsonSerializer.Deserialize<IEnumerable<TargetModel>>(
                content,
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
                );
            return targets;
        }


        public async Task<AgentModel> GetTArgetByIdAsync(int id)
        {
            var response = await httpClient.GetAsync($"{baseUrl}/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var target = JsonSerializer.Deserialize<AgentModel>(
                content,
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
                );
            return target;
        }
    }
}
