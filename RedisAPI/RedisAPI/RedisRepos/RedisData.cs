using RedisAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace RedisAPI.RedisRepos;

public abstract class RedisData
{
    [Required]
    public string Id { get; set; } = Guid.NewGuid().ToString();
}
