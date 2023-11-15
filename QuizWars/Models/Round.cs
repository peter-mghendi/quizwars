namespace QuizWars.Models;

public class Round
{
    public long Id { get; set; }

    public int Index { get; set; }

    public required Game Game { get; set; }

    public required Question Question { get; set; }

    public List<Response> Responses { get; } = new();
}