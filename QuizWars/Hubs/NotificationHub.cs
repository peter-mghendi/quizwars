using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using QuizWars.Hubs.Clients;

namespace QuizWars.Hubs;

[Authorize]
public class NotificationHub : Hub<INotificationHubClient>;