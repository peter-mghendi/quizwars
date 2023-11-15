using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuizWars.Models;

namespace QuizWars.Data;

public class ApplicationDbContext
    (DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Category> Categories => Set<Category>();

    public DbSet<Choice> Choices => Set<Choice>();

    public DbSet<Game> Games => Set<Game>();

    public DbSet<Question> Questions => Set<Question>();
    
    public DbSet<Response> Responses => Set<Response>();
    
    public DbSet<Round> Rounds => Set<Round>();
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<ApplicationUser>()
            .HasMany(e => e.CreatedGames)
            .WithOne(e => e.PlayerOne);
        
        modelBuilder.Entity<ApplicationUser>()
            .HasMany(e => e.InvitedGames)
            .WithOne(e => e.PlayerTwo);
        
        modelBuilder.Entity<Response>().HasOne(e => e.Choice);
        
        modelBuilder.Entity<Game>()
            .HasMany(e => e.Rounds)
            .WithOne(e => e.Game);
        
        modelBuilder.Entity<Question>()
            .HasMany(e => e.Choices)
            .WithOne(e => e.Question);
        
        modelBuilder.Entity<Round>().HasOne(e => e.Question);
        
        modelBuilder.Entity<Round>()
            .HasMany(e => e.Responses)
            .WithOne(e => e.Round);
        
        modelBuilder.Entity<Category>()
            .HasMany(e => e.Questions)
            .WithOne(e => e.Category);
    }
}