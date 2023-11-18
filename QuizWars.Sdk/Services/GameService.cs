using System.Net.Http.Json;
using QuizWars.Shared.Models.Request;
using QuizWars.Shared.Models.Response;

namespace QuizWars.Sdk.Services;

public class GameService(HttpClient client)
{
    public async Task<GameResponse> GetGame(Guid identifier)
    {
        var response = await client.GetFromJsonAsync<GameResponse>($"api/games/{identifier}");
        return response!;
    } 
    
    public async Task<GameResponse> CreateGame(CreateGameRequest request)
    {
        var response = await client.PostAsJsonAsync("api/games", request);
        var game = await response.Content.ReadFromJsonAsync<GameResponse>();
        return game!;
    } 
}