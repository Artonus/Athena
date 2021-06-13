using Athena.API.Jobs;
using Athena.API.Services;
using Athena.DataAccess;
using Athena.DataAccess.Repository;
using Athena.DataAccess.Repository.Base;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Quartz;
using StackExchange.Redis;

namespace Athena.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; set;}

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Athena", Version = "v1"});
            });
            services.AddDbContext<AthenaDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
            );
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddTransient(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IRetailerRepository, RetailerRepository>();
            services.AddTransient<ICrawler, Crawler>();
            services.AddTransient<IStockService, StockService>();
            

            // Redis
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(Configuration.GetConnectionString("Redis")));

            // Quartz scheduler
            services.AddQuartz();
            services.AddTransient<CrawlerJob>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Athena v1"));
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}