using System;
using System.Linq;
using Athena.API.Services;
using Athena.DataAccess;
using Athena.DataAccess.Repository;
using FluentScheduler;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Athena.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            CreateDatabaseIfNotExists(host);
            
            JobManager.Initialize();

            JobManager.AddJob(
                () => Stock.Check(),
                s => s.ToRunEvery(5).Minutes()
            );

            Stock.Check();

            host.Run();
        }

        private static void CreateDatabaseIfNotExists(IHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<AthenaDbContext>();
                    //context.Database.EnsureCreated();
                    DbInitializer.Initialize(context);
                    var repo = services.GetService<IProductRepository>();
                    var templates = repo.GetAll().Select(s => s.AccessTemplate).ToList();
                    Stock.Init(templates);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
