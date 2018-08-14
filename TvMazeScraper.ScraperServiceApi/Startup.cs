using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using TvMazeScraper.Common.Exception;
using TvMazeScraper.Common.Helper;
using TvMazeScraper.Data;
using TvMazeScraper.Data.Read;
using TvMazeScraper.Data.Write;
using TvMazeScraper.ScraperServices;
using TvMazeScraper.ScraperServices.Scraper;
using TvMazeScraper.ScraperServices.TvMazeClient;
using TvMazeScraper.ScraperServices.TvMazeClient.Configuration;

namespace TvMazeScraper.ScraperServiceApi
{
    public class Startup
    {
        private const string DbConnectionString = "SqlServerConnection";
        private const string AzureKey = "Azure";
        private const string ServiceBusConnectionString = nameof(ServiceBusConnectionString);
        private const string ServiceBusQueueName = nameof(ServiceBusQueueName);
        private const string MigrationAssemblyName = "TvMazeScraper.Data";
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
            services.AddScoped<IScraperService, ScraperService>();
            services.AddScoped<IActionTypeAssistant, NewDataAssistant>();
            services.AddScoped<IActionTypeAssistant, ExistingDataAssistant>();
            services.AddScoped<ITvMazeWebClient, TvMazeWebClient>();
            services.AddScoped<ITvMazeHttpClient, TvMazeHttpClient>();
            services.AddScoped<ITvMazeDataManipulator, TvMazeDataInjector>();
            services.AddScoped<ITvMazeDataManipulator, TvMazeDataUpdater>();
            services.AddSingleton<IDataParse, DataParse>();
            services.AddScoped<ITvMazeDataReader, TvMazeDataReader>();
            services.AddSingleton<IRestClient, RestClient>();
            services.AddSingleton<TvMazeApiConfiguration>();
            services.AddHostedService<ScraperRunnerHostedService>();
            InitializeServiceBus(services);
        }

        private void InitializeServiceBus(IServiceCollection services)
        {
            var azureSection = _configuration.GetSection(AzureKey);
            IQueueClient queueClient = new QueueClient(azureSection[ServiceBusConnectionString], azureSection[ServiceBusQueueName]);
            services.AddSingleton(queueClient);
            services.AddSingleton<IServiceBusListener, ServiceBusListener>();
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
                        optionsBuilder.MigrationsAssembly(MigrationAssemblyName);
                    });
            });
        }
    }
}
