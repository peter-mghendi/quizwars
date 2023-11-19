using System.Net.Http.Json;
using QuizWars.Shared.Models.Request;
using QuizWars.Shared.Models.Response;

namespace QuizWars.Sdk.Services;

public class ResponseService(HttpClient client)
{
    public async Task<List<ResponseResponse>> CreateResponse(Guid identifier, int index, CreateResponseRequest request)
    {
        var result = await client.PostAsJsonAsync($"api/games/{identifier}/rounds/{index}/responses", request);
        var response = await result.Content.ReadFromJsonAsync<List<ResponseResponse>>();
        return response!;
    } 
}