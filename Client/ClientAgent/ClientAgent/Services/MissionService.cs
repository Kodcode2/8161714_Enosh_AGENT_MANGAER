using System.Text.Json;
using ClientAgent.ViewModel;
using NuGet.Protocol;

namespace ClientAgent.Services
{
    public class MissionService(HttpClient httpClient)
    {
        private readonly string baseUrl = "https://localhost:7244/missions";


        public async Task<IEnumerable<MissionVM>> GetAllMissionsAsync()
        {
            var response = await httpClient.GetAsync(baseUrl);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var missions = JsonSerializer.Deserialize<IEnumerable<MissionVM>>(
                content,
                    new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }
                );
            return missions;
        }


        public async Task<MissionVM?> GetMissionByIdAsync(int id)
        {
            var missions = await GetAllMissionsAsync();
            var mission = missions.FirstOrDefault(m => m.Id == id);
            return mission;
        }


        public async Task AssignMissionAsync(int id)
        {
            var response = await httpClient.PutAsync($"{baseUrl}/{id}", null);
            response.EnsureSuccessStatusCode();
        }
    }
}
