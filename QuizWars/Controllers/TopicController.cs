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
[Route("api/topics")]
public class TopicController(ApplicationDbContext context) : ControllerBase
{
    // GET: api/topics
    [HttpGet]
    [AllowAnonymous]
    public async Task<List<TopicResponse>> GetTopics()
    {
        return await context.Topics.Select(topic => topic.AsResponse()).ToListAsync();
    }

    // GET: api/topics/5
    [HttpGet("{id}")]
    public async Task<ActionResult<TopicResponse>> GetTopic(long id)
    {
        var topic = await context.Topics.FindAsync(id);
        if (topic is null)
        {
            return NotFound();
        }

        return topic.AsResponse();
    }

    // PUT: api/topics/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTopic(long id, Topic topic)
    {
        if (id != topic.Id)
        {
            return BadRequest();
        }

        context.Entry(topic).State = EntityState.Modified;

        try
        {
            await context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException) when (!TopicExists(id))
        {
            return NotFound();
        }

        return NoContent();
    }

    // POST: api/topics
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<TopicResponse>> PostTopic(Topic topic)
    {
        context.Topics.Add(topic);
        await context.SaveChangesAsync();

        return CreatedAtAction("GetTopic", new { id = topic.Id }, topic.AsResponse());
    }

    // DELETE: api/topics/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTopic(long id)
    {
        var topic = await context.Topics.FindAsync(id);
        if (topic == null)
        {
            return NotFound();
        }

        context.Topics.Remove(topic);
        await context.SaveChangesAsync();

        return NoContent();
    }

    private bool TopicExists(long id)
    {
        return context.Topics.Any(e => e.Id == id);
    }
}