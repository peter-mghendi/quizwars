using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using QuizWars.Data;
using QuizWars.Extensions;
using QuizWars.Hubs;
using QuizWars.Hubs.Clients;
using QuizWars.Models;
using QuizWars.Services;
using QuizWars.Shared.Models.Enum;
using QuizWars.Shared.Models.Request;
using QuizWars.Shared.Models.Response;

namespace QuizWars.Controllers;

[Authorize]
[ApiController]
[Route("api/games/{identifier:guid}/rounds/{index:int}/responses")]
public class ResponseController(
    ApplicationDbContext context,
    IHubContext<NotificationHub, INotificationHubClient> hub,
    UserManager<ApplicationUser> manager
) : ControllerBase
{
    // POST: api/games/0085f04e-8a30-449e-91e1-38899b4d3ed5/rounds/3/responses
    [HttpPost]
    public async Task<ActionResult<IEnumerable<ResponseResponse>>> PostResponse(
        Guid identifier,
        int index,
        [FromBody] CreateResponseRequest request
    )
    {
        var game = await GetGameAsync(identifier);
        if (game is null)
        {
            return NotFound();
        }

        var email = User.Identity!.Name!;

        // TODO: Inline list syntax
        var participants = new List<string> { game.PlayerOne.Email!, game.PlayerTwo.Email! };
        if (!participants.Contains(email))
        {
            return BadRequest();
        }

        var round = game.Rounds.SingleOrDefault(r => r.Index == index);
        if (round is null)
        {
            return BadRequest();
        }

        if (round.Responses.Any(r => r.User.Email! == email))
        {
            return Conflict();
        }

        var choice = round.Question.Choices.SingleOrDefault(c => c.Id == request.ChoiceId);
        if (choice is null && request.ChoiceId is not 0)
        {
            return BadRequest();
        }

        var user = await manager.FindByEmailAsync(email);
        var response = new Response
        {
            TimeLeft = request.TimeLeft,
            Points = CalculatePoints(round, choice, request.TimeLeft),
            Choice = choice,
            User = user!,
            Round = round,
        };

        round.Responses.Add(response);

        if (round.Index is 6 && email == game.PlayerTwo.Email)
        {
            var notification = new Notification
            {
                Action = NotificationAction.Results,
                SentAt = DateTimeOffset.UtcNow,
                Game = game,
                Recipient = game.PlayerOne
            };
            context.Notifications.Add(notification);
            
            await context.Entry(game.PlayerOne).Collection(u => u.NotificationSubscriptions).LoadAsync();
            await hub.Clients.User(notification.Recipient.Id).ReceiveNotification(notification.AsResponse());
            await PushNotificationService.SendNotificationAsync(notification);
        }

        await context.SaveChangesAsync();

        // User has responded, do not obfuscate the response.
        return round.Responses.Select(r => r.AsResponse()).ToList();
    }

    private async Task<Game?> GetGameAsync(Guid identifier) => await context.Games
        .Include(g => g.Topic)
        .Include(g => g.PlayerOne)
        .Include(g => g.PlayerTwo)
        .Include(g => g.Rounds)
        .ThenInclude(r => r.Question)
        .ThenInclude(q => q.Choices)
        .Include(g => g.Rounds)
        .ThenInclude(r => r.Responses)
        .ThenInclude(r => r.Choice)
        .Include(g => g.Rounds)
        .ThenInclude(r => r.Responses)
        .ThenInclude(r => r.User)
        .SingleOrDefaultAsync(g => g.Identifier == identifier);


    private static int CalculatePoints(Round round, Choice? choice, int time)
    {
        if (choice is null or { IsCorrect: false })
        {
            return 0;
        }

        var points = time + 10;
        return round.Index + 1 == GameService.GameRounds ? points * 2 : points;
    }
}