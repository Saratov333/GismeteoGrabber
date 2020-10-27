using System.Collections.Generic;
using System.IO;
using GismeteoGrabber.DAL.Repositories;
using GismeteoGrabber.DAL.Repositories.Abstractions;
using GismeteoGrabber.Domain.Mappers;
using GismeteoGrabber.Domain.Mappers.Abstractions;
using GismeteoGrabber.Domain.Services;
using GismeteoGrabber.Domain.Services.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace GismeteoGrabber.Tests.Helpers
{
    /// <summary>
    /// Helper to work with application configuration
    /// </summary>
    public static class TestHelper
    {
        private static readonly Dictionary<string, string> InMemoryTestConfiguration =
            new Dictionary<string, string>
            {
                {"DefaultMainSiteDataPath",  Directory.GetCurrentDirectory() + @"\TestData\testMainPageSiteData.html"},
                {"DefaultDelayTimeMinutes", "1"}
            };

        /// <summary>
        /// Get test host
        /// </summary>
        /// <returns></returns>
        public static IHost GetTestHost()
        {
            var builder = new HostBuilder()
                .ConfigureServices((hostingContext, services) =>
                {
                    services.AddScoped<IWeatherSiteDataGrabService, WeatherSiteDataGrabService>();
                    services.AddScoped<IWeatherSiteDataSaveService, WeatherSiteDataSaveService>();
                    services.AddScoped<ICityMapper, CityMapper>();
                    services.AddScoped<ICityWeatherInfoMapper, CityWeatherInfoMapper>();
                    services.AddScoped<IWeatherSiteDataFacade, WeatherSiteDataFacade>();
                    services.AddScoped<ICityRepository, CityRepository>();
                    services.AddScoped<ICityWeatherInfoRepository, CityWeatherInfoRepository>();
                })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.Sources.Clear();
                    config.AddInMemoryCollection(InMemoryTestConfiguration);
                });
               
            var host = builder.Build();

            return host;
        }
    }
}
