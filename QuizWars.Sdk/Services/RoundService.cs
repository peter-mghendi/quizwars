using System.Net.Http.Json;
using QuizWars.Shared.Models.Response;

namespace QuizWars.Sdk.Services;

public class RoundService(HttpClient client)
{
    public async Task<List<RoundResponse>> GetRounds(Guid identifier)
    {
        var response = await client.GetFromJsonAsync<List<RoundResponse>>($"api/games/{identifier}/rounds");
        return response!;
    }
    
    public async Task<RoundResponse> GetRound(Guid identifier, int index)
    {
        var response = await client.GetFromJsonAsync<RoundResponse>($"api/games/{identifier}/rounds/{index}");
        return response!;
    } 
}