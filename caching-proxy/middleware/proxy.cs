using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Net.Http;

public class Proxy{
    private readonly RequestDelegate _next;
    private readonly HttpClient _client;
    private readonly Cache _cache;
    private const int ttl = 60;

    public Proxy(RequestDelegate next, Cache cache){
        _next = next;
        _client = new HttpClient();
        _cache = cache;
    }

    public async Task Invoke(HttpContext context){
        var key = context.Request.Path;
        var cachedResponse = await _cache.GetCacheResponseAsync(key);
        if(cachedResponse != null){
            context.Response.Headers.Add("X-Cache", "HIT");
            await context.Response.WriteAsync(cachedResponse);
            return;
        }

        var response = await _client.GetAsync("http://localhost:5000" + key);
        var responseString = await response.Content.ReadAsStringAsync();

        if(response.IsSuccessStatusCode){
        context.Response.Headers.Add("X-Cache", "MISS");
        await _cache.SetCacheResponseAsync(key, responseString, ttl);
        }
        await context.Response.WriteAsync(responseString);
        context.Response.StatusCode = (int)response.StatusCode;
    }
}