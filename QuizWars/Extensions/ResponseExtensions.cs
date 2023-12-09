using QuizWars.Client;
using QuizWars.Data;
using QuizWars.Models;
using QuizWars.Shared.Models;
using QuizWars.Shared.Models.Request;
using QuizWars.Shared.Models.Response;

namespace QuizWars.Extensions;

public static class ResponseExtensions
{
    public static ChoiceResponse AsResponse(this Choice choice) => new(choice.Id, choice.Text, choice.IsCorrect);

    public static GameResponse AsResponse(this Game game) => new(
        game.Id,
        game.Identifier,
        game.Topic.AsResponse(),
        game.PlayerOne.AsUserInfo(),
        game.PlayerTwo.AsUserInfo()
    );

    public static NotificationResponse AsResponse(this Notification notification)
    {
        return new NotificationResponse(
            notification.Id,
            notification.Action,
            notification.SentAt,
            notification.ReadAt,
            notification.Game.AsResponse()
        );
    }

    public static NotificationSubscriptionData AsResponse(this NotificationSubscription subscription)
    {
        return new NotificationSubscriptionData{
            NotificationSubscriptionId = subscription.Id,
            UserId = subscription.User.Email,
            Url = subscription.Url,
            P256dh = subscription.P256dh,
            Auth = subscription.Auth
        };
    }

    public static QuestionResponse AsResponse(this Question question) => new(
        question.Id,
        question.Text,
        question.Choices.Select(c => c.AsResponse()).ToList()
    );

    public static ResponseResponse AsResponse(this Response response) => new(
        response.Id,
        response.TimeLeft,
        response.Points,
        response.Choice?.AsResponse(),
        response.User.AsUserInfo()
    );

    public static RoundResponse AsResponse(this Round round) => new(
        round.Id,
        round.Index,
        round.Question.AsResponse(),
        round.Responses.Select(r => r.AsResponse()).ToList()
    );

    public static TopicResponse AsResponse(this Topic topic) => new(topic.Id, topic.Title, topic.Description);

    public static UserInfo AsUserInfo(this ApplicationUser user) => new() { UserId = user.Id, Email = user.Email! };
}