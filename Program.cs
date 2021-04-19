using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Channels;
using MessagingChannels.Example;
using MessagingChannels.Example.Common;
using Microsoft.AspNetCore.Routing;

CreateHostBuilder(args).Build().Run();

static IHostBuilder CreateHostBuilder(string[] args) =>
    Host
    .CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder => webBuilder
        .ConfigureServices((_, services) =>
        {
            services.AddSingleton(_ => Channel.CreateUnbounded<object>());
            services.AddSingleton<Producer>();
            services.AddHostedService<Consumer>();
        })
        .Configure((_, app) =>
        {
            app.UseRouting();
            app.UseEndpoints(route =>
            {
                route.MapGet("/", async context =>
                {
                    var producer = app.ApplicationServices.GetRequiredService<Producer>();
                    producer.Send(new Message<string>() { Body = "OK" });
                    await context.Response.WriteAsync("Hello world");
                });
            });
        }));