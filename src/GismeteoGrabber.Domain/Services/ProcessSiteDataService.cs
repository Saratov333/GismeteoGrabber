using System;
using System.Threading;
using System.Threading.Tasks;
using GismeteoGrabber.Domain.Options;
using GismeteoGrabber.Domain.Services.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GismeteoGrabber.Domain.Services
{
    /// <summary>
    /// Process data hosted service
    /// </summary>
    public class ProcessSiteDataService : BackgroundService
    {
        private readonly IWeatherSiteDataFacade _weatherSiteDataFacade;
        private readonly ILogger<ProcessSiteDataService> _logger;
        private readonly string _processSiteUrl;
        private readonly TimeSpan _delayForProcessInMinutes;

        /// <summary>
        /// Process data hosted service
        /// </summary>
        public ProcessSiteDataService(
            IOptions<GismeteoGrabberOptions> gismeteoGrabberOptions,
            IWeatherSiteDataFacade weatherSiteDataFacade,
            ILogger<ProcessSiteDataService> logger)
        {
            _weatherSiteDataFacade = weatherSiteDataFacade;
            _logger = logger;
            _processSiteUrl = gismeteoGrabberOptions.Value.DefaultSiteDataUrl;
            _delayForProcessInMinutes = new TimeSpan(0, gismeteoGrabberOptions.Value.DefaultDelayTimeMinutes, 0);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Process data task started");

            stoppingToken.Register(() =>
                _logger.LogInformation($"Process data task stopped"));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Process site data started");

                await _weatherSiteDataFacade.StartProcessSiteDataAsync(stoppingToken, _processSiteUrl);

                _logger.LogInformation($"Process site data successfully ended");

                await Task.Delay(_delayForProcessInMinutes, stoppingToken);
            }

            _logger.LogInformation($"Process data task stopped");
        }
    }
}
