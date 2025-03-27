using DotNetEnv;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CommandLine;

public class Program
{
    public static void Main(string[] args)
    {
        Env.Load();
        Console.WriteLine("Starting caching proxy server...");
        var port = 3000;
        Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(o => port = o.Port);

        var host = host.CreateDefaultBuilder()
            .ConfigureServices(services =>services.AddSingleton(new Cache(Environment.GetEnvironmentVariable("AWS_ELASTIC_CACHE_ENDPOINT"))))
            .ConfigureWebHostDefaults(webbuilder =>{
                webbuilder.Configure(app => app.UseMiddleware<Proxy>());
                webbuilder.UseUrls($"http://localhost:{port}");
            })
            
            .Build();

            host.Run();
    }
    class Options{
        [Option('p', "port", Required = false, Default = 3000, HelpText = "Port to listen on")]
        public int Port { get; set; }
    }
}