namespace QuizWars.Shared.Models.Response;

public record QuestionResponse(long Id, string Text, List<ChoiceResponse> Choices);