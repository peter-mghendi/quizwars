using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizWars.Data;
using QuizWars.Extensions;
using QuizWars.Models;
using QuizWars.Services;
using QuizWars.Shared.Models.Request;
using QuizWars.Shared.Models.Response;

namespace QuizWars.Controllers;

[Authorize]
[ApiController]
[Route("api/games")]
public class GameController(ApplicationDbContext context, ILogger<GameController> logger,
    UserManager<ApplicationUser> manager) : ControllerBase
{
    // GET: api/games
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GameResponse>>> GetGames()
    {
        return await context.Games.Select(g => g.AsResponse()).ToListAsync();
    }

    // GET: api/games/0085f04e-8a30-449e-91e1-38899b4d3ed5
    [HttpGet("{identifier}")]
    public async Task<ActionResult<GameResponse>> GetGame(Guid identifier)
    {
        var game = await context.Games
            .Include(g => g.Topic)
            .Include(g => g.PlayerOne)
            .Include(g => g.PlayerTwo)
            .SingleOrDefaultAsync(g => g.Identifier == identifier);

        if (game is null)
        {
            return NotFound();
        }

        return game.AsResponse();
    }

    // PUT: api/games/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutGame(long id, Game game)
    {
        if (id != game.Id)
        {
            return BadRequest();
        }

        context.Entry(game).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!GameExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }

    // POST: api/games
    [HttpPost]
    public async Task<ActionResult<GameResponse>> PostGame(CreateGameRequest request, [FromServices] GameService service)
    {
        
        var userId = User.Identity!.Name!;
        var creator = await manager.FindByEmailAsync(userId);
        logger.LogInformation("User {UserId} is creating game in topic {TopicId}.", userId, request.TopicId);

        try
        {
            var game = await service.CreateGame(creator!, request);
            return CreatedAtAction("GetGame", new { identifier = game.Identifier }, game.AsResponse());
        }
        catch (BadHttpRequestException)
        {
            return BadRequest();
        }
    }

    // DELETE: api/games/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGame(long id)
    {
        var game = await context.Games.FindAsync(id);
        if (game == null)
        {
            return NotFound();
        }

        context.Games.Remove(game);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private bool GameExists(long id)
    {
        return context.Games.Any(e => e.Id == id);
    }
}