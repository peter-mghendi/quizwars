using QuizWars.Data;

namespace QuizWars.Models;

public class Game
{
    public long Id { get; set; }

    public required string Text { get; set; }

    public required Category Category { get; set; }

    public required ApplicationUser PlayerOne { get; set; }

    public required ApplicationUser PlayerTwo { get; set; }

    public List<Round> Rounds { get; } = new();
}