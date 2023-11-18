namespace QuizWars.Shared.Models.Request;

public record CreateGameRequest(long TopicId, string OpponentId);