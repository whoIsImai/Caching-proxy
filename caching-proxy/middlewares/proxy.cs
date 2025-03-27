using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Net.Http;

public class Proxy
{
    private readonly RequestDelegate _next;
    private readonly HttpClient _client;
    private readonly Cache _cache;
    private readonly proxyConfig _config;
    private const int ttl = 60;

    public Proxy(RequestDelegate next, proxyConfig config, Cache cache)
    {
        _next = next;
        _config = config;
        _client = new HttpClient();
        _cache = cache;
    }

    public async Task Invoke(HttpContext context)
    {
        var key = context.Request.Path + context.Request.QueryString;
        var cachedResponse = await _cache.GetCacheResponseAsync(key);
        if(cachedResponse != null)
        {
            context.Response.Headers["X-Cache"] = "HIT";
            await context.Response.WriteAsync(cachedResponse);
            return;
        }

        var targetUrl = _config._TargetUrl + context.Request.Path + context.Request.QueryString;
        var response = await _client.GetAsync(targetUrl);
        var responseString = await response.Content.ReadAsStringAsync();

        if(response.IsSuccessStatusCode)
        {
            context.Response.Headers["X-Cache"]=  "MISS";
            await _cache.SetCacheResponseAsync(key, responseString, ttl);
            await context.Response.WriteAsync(responseString);
            context.Response.StatusCode = (int)response.StatusCode;
        } else{
            context.Response.Headers["X-Cache"] = "ERROR";
            context.Response.StatusCode = (int)response.StatusCode;
            await context.Response.WriteAsync("Error: " + response.ReasonPhrase);
        }
    }
}