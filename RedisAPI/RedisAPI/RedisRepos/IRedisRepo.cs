
namespace RedisAPI.RedisRepos;

public interface IRedisRepo<T> where T : RedisData
{
    Task CreateAsync(T item);
    Task<T?> GetByIdAsync(string id);
    Task<IEnumerable<T?>> GetAllAsync();
}
