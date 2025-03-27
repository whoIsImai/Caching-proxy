using DotNetEnv;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CommandLine;
using System;

public class Program
{
    public static void Main(string[] args)
    {
        Env.Load();

        var port = 3000;
        var targetUrl = "";

        Parser.Default.ParseArguments<Options>(args)
            .WithParsed<Options>(o =>{
                 port = o.Port
                 targetUrl = o.TargetUrl;
                 });
       
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services => {
                services.AddSingleton(new Cache(Environment.GetEnvironmentVariable("AWS_ELASTIC_CACHE_ENDPOINT")))
                services.AddSingleton(new proxyConfig(targetUrl));
                })
            .ConfigureWebHostDefaults(webbuilder =>{
                webbuilder.Configure(app => app.UseMiddleware<Proxy>());
                webbuilder.UseUrls($"http://localhost:{port}");
            })
            
            .Build();
                Console.WriteLine($"Proxy started on http://localhost:{port}, forwarding to {targetUrl}");
            host.Run();
    }
    class Options{
        [Option('p', "port", Required = false, Default = 3000, HelpText = "Port to listen on")]
        public int Port { get; set; }

        [Option('t', "target", Required = true, HelpText = "The target server URL to forward requests to")]
        public string TargetUrl { get; set; }
    }
}