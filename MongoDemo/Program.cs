using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using Microsoft.Extensions.Configuration;
namespace MongoDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = CreateDefaultBuilder().Build();
             IServiceScope serviceScope = host.Services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;
            var workerInstance = provider.GetRequiredService<Worker>();
            workerInstance.MongoDbCrud("571fd15840c4ed0d240d7d6c", "JobInterview", "TAKE-ME!");
            host.Run();
        }

        static IHostBuilder CreateDefaultBuilder()
        {
            return Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(app =>
                {
                    app.AddJsonFile("appsettings.json");
                })
                .ConfigureServices(services =>
                {
                    services.AddSingleton<Worker>();
                });
        }
    }
}
