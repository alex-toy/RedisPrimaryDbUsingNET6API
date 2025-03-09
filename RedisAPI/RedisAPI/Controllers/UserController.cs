using Microsoft.AspNetCore.Mvc;
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
        User? user = await _redisRepo.GetByIdAsync(id);
        if (user is not null) return Ok(user);
        return NotFound();
    }

    [HttpPost]
    public async Task<ActionResult<Platform>> Create(User user)
    {
        await _redisRepo.CreateAsync(user);
        return CreatedAtRoute(nameof(GetById), new { user.Id }, user);
    }
}
