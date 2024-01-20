namespace QuizWars.Shared.Models.Response;

public record ResponseResponse(long Id, int TimeLeft, int Points, ChoiceResponse? Choice, UserInfo User);