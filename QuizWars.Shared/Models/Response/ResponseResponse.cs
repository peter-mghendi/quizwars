using QuizWars.Client;

namespace QuizWars.Shared.Models.Response;

public record ResponseResponse(long Id, int? Duration, ChoiceResponse? Choice, UserInfo User);