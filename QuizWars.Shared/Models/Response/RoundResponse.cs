namespace QuizWars.Shared.Models.Response;

public record RoundResponse(long Id, int Index, QuestionResponse Question, List<ResponseResponse> Responses);