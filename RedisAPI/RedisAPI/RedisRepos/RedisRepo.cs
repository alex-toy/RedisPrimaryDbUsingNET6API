using RedisAPI.Exceptions;
using StackExchange.Redis;
using System.Text.Json;

namespace RedisAPI.RedisRepos;

public class RedisRepo<T> : IRedisRepo<T> where T : RedisData
{
    private readonly IConnectionMultiplexer _redis;
    private readonly IDatabase _db;

    public RedisRepo(IConnectionMultiplexer redis)
    {
        _redis = redis;
        _db = _redis.GetDatabase();
    }

    public async Task CreateAsync(T item)
    {
        if (item is null) throw new ArgumentOutOfRangeException(nameof(item));
        string key = $"{typeof(T)}:{item.Id}";
        string serializedItem = JsonSerializer.Serialize(item);
        await _db.StringSetAsync(key, serializedItem);
    }

    public async Task<IEnumerable<T?>> GetAllAsync()
    {
        string prefix = $"{typeof(T)}";
        RedisResult keys = await _db.ScriptEvaluateAsync("return redis.call('keys', ARGV[1])", [prefix], [$"{prefix}:*"]);

        List<T?> items = new ();

        foreach (var key in (RedisKey[])keys)
        {
            RedisValue data = await _db.StringGetAsync(key);
            if (data.HasValue)
            {
                var item = JsonSerializer.Deserialize<T>(data);
                items.Add(item);
            }
        }

        return items;
    }

    public async Task<T?> GetByIdAsync(string id)
    {
        string key = $"{typeof(T)}:{id}";
        RedisValue data = await _db.StringGetAsync(key);

        if (!data.HasValue) throw new UnexistingItem(id);

        return JsonSerializer.Deserialize<T>(data);
    }

    public async Task UpdateAsync(T item)
    {
        string key = $"{typeof(T)}:{item.Id}";
        bool exists = await _db.KeyExistsAsync(key);

        if (!exists) throw new UnexistingItem(item.Id);

        string serializedItem = JsonSerializer.Serialize(item);
        await _db.StringSetAsync(key, serializedItem);
    }

    public async Task DeleteAsync(string id)
    {
        string key = $"{typeof(T)}:{id}";
        bool exists = await _db.KeyExistsAsync(key);

        if (!exists) throw new UnexistingItem(id);

        await _db.KeyDeleteAsync(key);
    }
}
