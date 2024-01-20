using Microsoft.AspNetCore.Identity;
using QuizWars.Models;

namespace QuizWars.Data;

// Add profile data for application users by adding properties to the ApplicationUser class
public class ApplicationUser : IdentityUser
{
    public List<Game> CreatedGames { get; } = [];
    
    public List<Game> InvitedGames { get; } = [];
    
    public List<NotificationSubscription> NotificationSubscriptions { get; } = [];
}

