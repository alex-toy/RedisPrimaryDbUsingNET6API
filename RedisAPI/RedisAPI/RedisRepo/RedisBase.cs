using StackExchange.Redis;
using System.Text.Json;

namespace RedisAPI.RedisRepo
{
    public class RedisBase<T> : IRedisBase<T> where T : RedisData
    {
        private readonly IConnectionMultiplexer _redis;

        public RedisBase(IConnectionMultiplexer redis)
        {
            _redis = redis;
        }
        public void Create(T item)
        {
            if (item is null)
            {
                throw new ArgumentOutOfRangeException(nameof(item));
            }

            var db = _redis.GetDatabase();

            var serialPlat = JsonSerializer.Serialize(item);

            //db.StringSet(plat.Id, serialPlat);
            db.HashSet($"hashplatform", new HashEntry[] { new HashEntry(item.Id, serialPlat)});
        }

        public IEnumerable<T?>? GetAll()
        {
            var db = _redis.GetDatabase();

            var completeSet = db.HashGetAll("hashplatform");

            if (completeSet.Length > 0)
            {
                var obj = Array.ConvertAll(completeSet, val => JsonSerializer.Deserialize<T>(val.Value)).ToList();
                return obj;
            }

            return null;
        }

        public T? GetById(string id)
        {
            var db = _redis.GetDatabase();

            //var plat = db.StringGet(id);

            RedisValue plat = db.HashGet("hashplatform", id);

            if (!string.IsNullOrEmpty(plat))
            {
                return JsonSerializer.Deserialize<T>(plat);
            }
            return default;
        }
    }
}
