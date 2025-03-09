using RedisAPI.Models;

namespace RedisAPI.RedisRepo;

public interface IRedisBase<T> where T : RedisData
{
    void Create(T item);
    T? GetById(string id);
    IEnumerable<T?>? GetAll();
}
