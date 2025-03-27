using StackExchange.Redis;
using System.Threading.Tasks;

public class Cache{
    private readonly IDatabase _redis;
    public Cache(string redis){
        var connection = ConnectionMultiplexer.Connect(redis);
        _redis = connection.GetDatabase();
    }

    public async Task<string?> GetCacheResponseAsync(string key){
        return await _redis.StringGetAsync(key);
    }

    public async Task SetCacheResponseAsync(string key, string value, int ttl){
        await _redis.StringSetAsync(key, value, expiry: new System.TimeSpan(0, 0, ttl));
    }
}