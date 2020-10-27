using System.IO;
using System.Threading.Tasks;
using GismeteoGrabber.DAL.Options;
using GismeteoGrabber.DAL.Repositories;
using GismeteoGrabber.DAL.Repositories.Abstractions;
using GismeteoGrabber.Domain.Mappers;
using GismeteoGrabber.Domain.Mappers.Abstractions;
using GismeteoGrabber.Domain.Options;
using GismeteoGrabber.Domain.Services;
using GismeteoGrabber.Domain.Services.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace GismeteoGrabber
{
    class Program
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static async Task Main(string[] args)
        {
            await new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", true, true);
                    if (args != null) config.AddCommandLine(args);
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    services.AddScoped<IWeatherSiteDataGrabService, WeatherSiteDataGrabService>();
                    services.AddScoped<IWeatherSiteDataSaveService, WeatherSiteDataSaveService>();
                    services.AddScoped<ICityMapper, CityMapper>();
                    services.AddScoped<ICityWeatherInfoMapper, CityWeatherInfoMapper>();
                    services.AddScoped<IWeatherSiteDataFacade, WeatherSiteDataFacade>();
                    services.AddScoped<ICityRepository, CityRepository>();
                    services.AddScoped<ICityWeatherInfoRepository, CityWeatherInfoRepository>();

                    services.AddHostedService<ProcessSiteDataService>();

                    services.Configure<GismeteoGrabberOptions>(
                        hostingContext.Configuration.GetSection("GismeteoGrabberOptions"));
                    services.Configure<GismeteoGrabberConnectOptions>(hostingContext.Configuration.GetSection("GismeteoGrabberConnectOptions"));
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddConsole(options =>
                    {
                        options.IncludeScopes = true;
                    });
                    logging.AddDebug();
                    logging.SetMinimumLevel(LogLevel.Trace);
                })
                .UseNLog()
                .Build()
                .RunAsync();
        }
    }
}
