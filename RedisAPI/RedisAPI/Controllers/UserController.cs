using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using RedisAPI.Exceptions;
using RedisAPI.Models;
using RedisAPI.RedisRepos;

namespace RedisAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IRedisRepo<User> _redisRepo;

    public UserController(IRedisRepo<User> repository)
    {
        _redisRepo = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<User?>>> GetAll()
    {
        IEnumerable<User?> users = await _redisRepo.GetAllAsync();
        return Ok(users);
    }

    [HttpGet("{id}", Name = "GetById")]
    public async Task<ActionResult<IEnumerable<Platform>>> GetById(string id)
    {
        try
        {
            User? user = await _redisRepo.GetByIdAsync(id);
            return Ok(user);
        }
        catch (UnexistingItem ex)
        {
            return NotFound(new { ex.Message, ex.ItemId });
        }
    }

    [HttpPost]
    public async Task<ActionResult<Platform>> Create(User user)
    {
        await _redisRepo.CreateAsync(user);
        return CreatedAtRoute(nameof(GetById), new { user.Id }, user);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] User user)
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
