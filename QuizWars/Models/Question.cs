namespace QuizWars.Models;

public class Question
{
    public long Id { get; set; }

    public required string Text { get; set; }

    public required Category Category { get; set; }

    public List<Choice> Choices { get; } = new();
}