using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizWars.Data;
using QuizWars.Extensions;
using QuizWars.Models;
using QuizWars.Shared.Models;
using QuizWars.Shared.Models.Response;

namespace QuizWars.Controllers;

[Authorize]
[ApiController]
[Route("api/notifications")]
public class NotificationController(ApplicationDbContext context, UserManager<ApplicationUser> manager) : ControllerBase
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
    [HttpPut("subscribe")]
    public async Task<IResult> PutNotification([FromBody] NotificationSubscriptionData data)
    {
        // We're storing at most one subscription per user, so delete old ones.
        // Alternatively, you could let the user register multiple subscriptions from different browsers/devices.
        var username = User.Identity!.Name;
        if (username is null)
        {
            return Results.Unauthorized();
        }
        
        var oldSubscriptions = context.NotificationSubscriptions.Where(e => e.User.Email == username);
        context.NotificationSubscriptions.RemoveRange(oldSubscriptions);

        var user = await manager.FindByEmailAsync(username);
        var subscription = new NotificationSubscription
        {
            Url = data.Url!,
            P256dh = data.P256dh!,
            Auth = data.Auth!,
            User = user!
        };
        
        // Store new subscription
        context.NotificationSubscriptions.Add(subscription);
        await context.SaveChangesAsync();
        
        return Results.Ok(subscription.AsResponse());
    }

    // PUT: api/notifications/5
    [HttpPut("{id:long}")]
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