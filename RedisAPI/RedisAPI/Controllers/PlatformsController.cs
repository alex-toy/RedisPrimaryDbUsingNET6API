using Microsoft.AspNetCore.Mvc;
using RedisAPI.Exceptions;
using RedisAPI.Models;
using RedisAPI.RedisRepos;

namespace RedisAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlatformsController : ControllerBase
{
    private readonly IRedisRepo<Platform> _redisRepo;

    public PlatformsController(IRedisRepo<Platform> repository)
    {
        _redisRepo = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Platform>>> GetPlatforms()
    {
        IEnumerable<Platform?> platforms = await _redisRepo.GetAllAsync();
        return Ok(platforms);
    }

    [HttpGet("{id}", Name = "GetPlatformById")]
    public async Task<ActionResult<IEnumerable<Platform>>> GetPlatformById(string id)
    {
        try
        {
            Platform? platform = await _redisRepo.GetByIdAsync(id);
            return Ok(platform);
        }
        catch (UnexistingItem ex)
        {
            return NotFound(new { ex.Message, ex.ItemId });
        }
    }

    [HttpPost]
    public async Task<ActionResult<Platform>> CreatePlatform(Platform platform)
    {
        await _redisRepo.CreateAsync(platform);
        return CreatedAtRoute(nameof(GetPlatformById), new { platform.Id }, platform);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] Platform user)
    {
        if (id != user.Id) return BadRequest("ID mismatch.");

        try
        {
            await _redisRepo.UpdateAsync(user);
            return Ok();
        }
        catch (UnexistingItem ex)
        {
            return NotFound(new { ex.Message, ex.ItemId });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await _redisRepo.DeleteAsync(id);
            return Ok();
        }
        catch (UnexistingItem ex)
        {
            return NotFound(new { ex.Message, ex.ItemId });
        }
    }
}
