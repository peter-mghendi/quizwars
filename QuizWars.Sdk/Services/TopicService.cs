using System.Net.Http.Json;
using QuizWars.Shared.Models.Response;

namespace QuizWars.Sdk.Services;

public class TopicService(HttpClient client)
{
    public async Task<List<TopicResponse>> GetTopics()
    {
        var response = await client.GetFromJsonAsync<List<TopicResponse>>("api/topics");
        return response!;
    } 
}