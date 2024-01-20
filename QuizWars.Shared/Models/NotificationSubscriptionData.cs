namespace QuizWars.Shared.Models;

public record NotificationSubscriptionData(
    long? NotificationSubscriptionId,
    string? UserId,
    string? Url,
    string? P256dh,
    string? Auth
);
