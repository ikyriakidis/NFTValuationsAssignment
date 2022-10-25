using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nethereum.Web3;
using NFTValuations.Domain.Models;
using NFTValuations.Domain.Services;
using Serilog;

namespace NFTValuations
{
    public static class Startup
    {
        public static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false);
            IConfiguration configuration = builder.Build();
            services.AddSingleton(configuration);

            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            Log.Information("Starting up..");

            services.AddLogging(builder =>
            {
                builder.AddSerilog();
            });

            var appSettingsConfigSection = configuration.GetSection(nameof(AppSettings));
            services.Configure<AppSettings>(appSettingsConfigSection);

            var appSettings = appSettingsConfigSection.Get<AppSettings>();

            services.AddSingleton<IWeb3>(web3 => new Web3($"https://mainnet.infura.io/v3/{appSettings.InfuraIoToken}"));
            services.AddSingleton<IServicePool, ServicePool>();

            services.AddHttpClient();

            services.AddSingleton<EntryPoint>();
            services.AddSingleton<IDetector, Detector>();

            return services;
        }
    }
}
