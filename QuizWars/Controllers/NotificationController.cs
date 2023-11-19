using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizWars.Data;
using QuizWars.Extensions;
using QuizWars.Models;
using QuizWars.Shared.Models.Response;

namespace QuizWars.Controllers;

[Authorize]
[ApiController]
[Route("api/notifications")]
public class NotificationController(ApplicationDbContext context) : ControllerBase
{
    // GET: api/notifications
    [HttpGet]
    public async Task<ActionResult<IEnumerable<NotificationResponse>>> GetNotifications()
    {
        return await context.Notifications
            .Include(n => n.Game)
            .ThenInclude(g => g.Topic)
            .Include(n => n.Game)
            .ThenInclude(g => g.PlayerOne)
            .Include(n => n.Game)
            .ThenInclude(g => g.PlayerTwo)
            .Where(n => n.Recipient.Email == User.Identity!.Name)
            .OrderByDescending(n => n.ReadAt == null)
            .ThenByDescending(n => n.SentAt)
            .Select(n => n.AsResponse())
            .ToListAsync();
    }

    // PUT: api/notifications/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutNotification(long id)
    {
        var notification = await context.Notifications.FindAsync(id);
        return notification switch
        {
            null => NotFound(),
            not { ReadAt: null } => Conflict(),
            _ => await MarkNotificationRead(notification)
        };
    }
    

    private async Task<NoContentResult> MarkNotificationRead(Notification notification)
    {
        notification.ReadAt = DateTimeOffset.UtcNow;
        await context.SaveChangesAsync();
        return NoContent();
    }
}