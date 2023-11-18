using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizWars.Data;
using QuizWars.Models;
using QuizWars.Shared.Models.Request;
using static System.Guid;

namespace QuizWars.Services;

public class GameService(ApplicationDbContext context, UserManager<ApplicationUser> manager)
{
    private const int GameRounds = 7;

    public async Task<Game> CreateGame(ApplicationUser creator, CreateGameRequest request)
    {
        var topic = await context.Topics
            .Include(t => t.Questions)
            .SingleOrDefaultAsync(t => t.Id == request.TopicId);
        
        if (topic is null)
        {
            throw new BadHttpRequestException("Topic not found.");
        }

        var opponent = await manager.FindByIdAsync(request.OpponentId);
        if (opponent is null)
        {
            throw new BadHttpRequestException("Opponent not found.");
        }

        var rounds = topic.Questions
            .OrderBy(_ => Random.Shared.Next())
            .Take(GameRounds)
            .Select((question, index) => new Round { Index = index, Question = question, })
            .ToList();

        var game = new Game
        {
            Identifier = NewGuid(),
            Topic = topic,
            PlayerOne = creator,
            PlayerTwo = opponent,
            Rounds = rounds
        };

        context.Games.Add(game);
        await context.SaveChangesAsync();

        return game;
    }
}