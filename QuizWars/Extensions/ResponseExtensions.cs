using QuizWars.Client;
using QuizWars.Data;
using QuizWars.Models;
using QuizWars.Shared.Models.Response;

namespace QuizWars.Extensions;

public static class ResponseExtensions
{
    public static GameResponse AsResponse(this Game game) => new(game.Id, game.Topic.AsResponse());
    public static TopicResponse AsResponse(this Topic topic) => new(topic.Id, topic.Title, topic.Description);

    public static UserInfo AsUserInfo(this ApplicationUser user) => new() { UserId = user.Id, Email = user.Email! };
}