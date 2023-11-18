using QuizWars.Data;

namespace QuizWars.Models;

public class Game
{
    public long Id { get; set; }

    public required Topic Topic { get; set; }

    public required ApplicationUser PlayerOne { get; set; }

    public required ApplicationUser PlayerTwo { get; set; }

    public List<Round> Rounds { get; init; } = new();
}