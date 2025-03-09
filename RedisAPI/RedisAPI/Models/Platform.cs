using RedisAPI.RedisRepos;
using System.ComponentModel.DataAnnotations;

namespace RedisAPI.Models;

public class Platform : RedisData
{
    [Required]
    public string Label { get; set; } = string.Empty;
}
