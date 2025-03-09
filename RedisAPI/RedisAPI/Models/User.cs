using RedisAPI.RedisRepos;

namespace RedisAPI.Models;

public class User : RedisData
{
    public string Name { get; set; } = string.Empty;
}
