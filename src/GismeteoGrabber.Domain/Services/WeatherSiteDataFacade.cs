using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GismeteoGrabber.Domain.Enums;
using GismeteoGrabber.Domain.Mappers.Abstractions;
using GismeteoGrabber.Domain.Services.Abstractions;
using Microsoft.Extensions.Logging;

namespace GismeteoGrabber.Domain.Services
{
    /// <summary>
    /// Weather process facade
    /// </summary>
    public class WeatherSiteDataFacade : IWeatherSiteDataFacade
    {
        private readonly IWeatherSiteDataGrabService _weatherSiteDataGrabService;
        private readonly IWeatherSiteDataSaveService _weatherSiteDataSaveService;
        private readonly ICityMapper _cityMapper;
        private readonly ILogger<WeatherSiteDataFacade> _logger;

        public WeatherSiteDataFacade(
            IWeatherSiteDataGrabService weatherSiteDataGrabService,
            IWeatherSiteDataSaveService weatherSiteDataSaveService,
            ICityMapper cityMapper,
            ILogger<WeatherSiteDataFacade> logger)
        {
            _weatherSiteDataGrabService = weatherSiteDataGrabService;
            _weatherSiteDataSaveService = weatherSiteDataSaveService;
            _cityMapper = cityMapper;
            _logger = logger;
        }

        /// <summary>
        /// Process weather site data
        /// </summary>
        public async Task StartProcessSiteDataAsync(CancellationToken cancellationToken, string siteUrl)
        {
            try
            {
                var siteData =
                    await _weatherSiteDataGrabService.GrabSiteWeatherByCitiesAsync(LoadHtmlSource.Web, siteUrl);
                var mappedData = siteData.Select(s => _cityMapper.Map(s)).ToList();
                var isResultSaved = await _weatherSiteDataSaveService.SaveWeatherDataAsync(mappedData);

                if (!isResultSaved)
                    _logger.LogError("Error in schedule saving weather data");
                else
                    _logger.LogInformation("Schedule successfully completed");

            }
            catch (Exception e)
            {
                _logger.LogError($"WeatherSiteDataFacade StartProcessSiteDataAsync error - [{e}]");
            }
        }
    }
}

