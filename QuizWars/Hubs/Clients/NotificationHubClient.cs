using QuizWars.Shared.Models.Response;

namespace QuizWars.Hubs.Clients;

public interface INotificationHubClient
{
    Task ReceiveNotification(NotificationResponse response);
}