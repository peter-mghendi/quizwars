using System.Net.Http.Json;
using QuizWars.Shared.Models.Response;

namespace QuizWars.Sdk.Services;

public class UserService(HttpClient client)
{
    public async Task<List<UserInfo>> GetUsers()
    {
        var response = await client.GetFromJsonAsync<List<UserInfo>>("api/users");
        return response!;
    } 
}