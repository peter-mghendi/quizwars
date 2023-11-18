using System.Net.Http.Json;
using QuizWars.Shared.Models.Request;
using QuizWars.Shared.Models.Response;

namespace QuizWars.Sdk.Services;

public class GameService(HttpClient client)
{
    public async Task CreateGame(CreateGameRequest request)
    {
        await client.PostAsJsonAsync("api/games", request);
    } 
}