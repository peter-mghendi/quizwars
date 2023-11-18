using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuizWars.Data;
using QuizWars.Models;
using QuizWars.Shared.Models.Request;

namespace QuizWars.Services;

public class GameService(ApplicationDbContext context, ILogger<GameService> logger, UserManager<ApplicationUser> manager)
{
    public async Task<Game> CreateGame(ApplicationUser creator, CreateGameRequest request)
    {
        Topic topic;
        try
        {
            topic = await context.Topics
                .Include(t => t.Questions)
                .SingleAsync(t => t.Id == request.TopicId);
        }
        catch (InvalidOperationException)
        {
            throw new BadHttpRequestException("Topic not found.");
        }
        
        var playerTwo = await manager.FindByIdAsync(request.OpponentId);
        
        var rounds = topic.Questions
            .OrderBy(_ => Random.Shared.Next())
            .Take(7)
            .Select((question, index) => new Round
            {
                Index = index,
                Question = question,
            }).ToList();

        var game = new Game
        {
            Topic = topic,
            PlayerOne = creator,
            PlayerTwo = playerTwo!,
            Rounds = rounds
        };

        context.Games.Add(game);
        await context.SaveChangesAsync();

        return game;
    }
}