using QuizWars.Data;

namespace QuizWars.Models;

public class Response
{
    public long Id { get; set; }

    public int? Duration { get; set; }
    
    public required ApplicationUser User { get; set; }

    public Choice? Choice { get; set; }

    public required Round Round { get; set; }
}