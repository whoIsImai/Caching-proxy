using StackExchange.Redis;
using System.Threading.Tasks;

public class Cache{
    private readonly IDatabase _redis;
    public Cache(string redis){
        var connection = ConnectionMultiplexer.Connect(redis);
        _redis = connection.GetDatabase();
    }

    public async Task<string> GetCacheResponseAsync(string key){
        var db = _redis.GetDatabase();
        return await db.StringGetAsync(key);
    }

    public async Task SetCacheResponseAsync(string key, string value, int ttl){
        var db = _redis.GetDatabase();
        await db.StringSetAsync(key, value, expiry: Timestamp.FromSeconds(ttl));
    }
}