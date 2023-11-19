using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuizWars.Client;
using QuizWars.Data;
using QuizWars.Extensions;

namespace QuizWars.Controllers;

[Authorize]
[ApiController]
[Route("api/users")]
public class UserController(UserManager<ApplicationUser> manager) : ControllerBase
{
    // GET: api/users
    [HttpGet]
    public async Task<IEnumerable<UserInfo>> GetUsers()
    {
        Console.WriteLine(User.Identity!.Name);
        return await manager.Users
            .Where(u => u.Email != User.Identity!.Name)
            .Select(u => u.AsUserInfo())
            .ToListAsync();
    }

    // GET: api/topics/5
    [HttpGet("{id}")]
    public async Task<ActionResult<UserInfo>> GetUser(string id)
    {
        var user = await manager.FindByIdAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        return user.AsUserInfo();
    }
}