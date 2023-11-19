using System.Net.Http.Json;
using QuizWars.Shared.Models.Request;
using QuizWars.Shared.Models.Response;

namespace QuizWars.Sdk.Services;

public class NotificationService(HttpClient client)
{
    public async Task<List<NotificationResponse>> GetNotifications()
    {
        var response = await client.GetFromJsonAsync<List<NotificationResponse>>("api/notifications");
        return response!;
    }

    public async Task MarkNotificationRead(long id)
    {
        _ = await client.PutAsync($"api/notifications/{id}", content: null);
    }
}