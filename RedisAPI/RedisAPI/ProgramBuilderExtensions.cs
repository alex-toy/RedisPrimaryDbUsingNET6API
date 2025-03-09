using RedisAPI.RedisRepos;
using StackExchange.Redis;

namespace RedisAPI;

public static class ProgramBuilderExtensions
{
    public static void ConfigureRedis(this WebApplicationBuilder builder)
    {
        string redisConnection = builder.Configuration.GetConnectionString("DockerRedisConnection")!;
        builder.Services.AddSingleton<IConnectionMultiplexer>(opt => ConnectionMultiplexer.Connect(redisConnection));
        builder.Services.AddScoped(typeof(IRedisRepo<>), typeof(RedisRepo<>));
    }
}
