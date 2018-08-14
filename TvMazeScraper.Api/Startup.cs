using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TvMazeScraper.ApiServices;
using TvMazeScraper.Common.Exception;
using TvMazeScraper.Common.Helper;
using TvMazeScraper.Data;
using TvMazeScraper.Data.Read;

namespace TvMazeScraper.Api
{
    public class Startup
    {
        private const string DbConnectionString = "DefaultConnection";
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            RegisterDependencies(services);
            ConfigureDatabase(services);
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseExceptionHandler(GlobalExceptionHandlerOptions.Configure);
            app.UseMvc();
        }

        private void RegisterDependencies(IServiceCollection services)
        {
            services.AddScoped<IShowService, ShowService>();
            services.AddSingleton<IDataParse, DataParse>();
            services.AddScoped<ITvMazeDataReader, TvMazeDataReader>();
        }

        private void ConfigureDatabase(IServiceCollection services)
        {
            var connsectionString = _configuration.GetConnectionString(DbConnectionString);
            services.AddDbContext<TvMazeScraperDbContext>(options =>
                {
                    options.UseSqlServer(
                        connsectionString, 
                        optionsBuilder =>
                        {
                            optionsBuilder.CommandTimeout(10);
                            optionsBuilder.MigrationsAssembly("TvMazeScraper.Data");
                        });
                });
        }
    }
}
