using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GismeteoGrabber.DAL.Constants;
using GismeteoGrabber.DAL.Models;
using GismeteoGrabber.DAL.Repositories.Abstractions;
using GismeteoGrabber.Domain.Services.Abstractions;
using Microsoft.Extensions.Logging;

namespace GismeteoGrabber.Domain.Services
{
    /// <summary>
    /// Weather save service
    /// </summary>
    public class WeatherSiteDataSaveService : IWeatherSiteDataSaveService
    {
        private readonly ICityRepository _cityRepository;
        private readonly ICityWeatherInfoRepository _cityWeatherInfoRepository;
        private readonly ILogger<WeatherSiteDataGrabService> _logger;

        /// <summary>
        /// Weather save service
        /// </summary>
        public WeatherSiteDataSaveService(
            ICityRepository cityRepository,
            ICityWeatherInfoRepository cityWeatherInfoRepository,
            ILogger<WeatherSiteDataGrabService> logger)
        {
            _cityRepository = cityRepository;
            _cityWeatherInfoRepository = cityWeatherInfoRepository;
            _logger = logger;
        }

        /// <summary>
        /// Save weather data to DB
        /// </summary>
        public async Task<bool> SaveWeatherDataAsync(List<DbCity> cities)
        {
            try
            {
                var existedCitiesDb =
                    await _cityRepository.GetExistedCitiesAsync(cities.Select(c => c.Name).ToList());

                if (existedCitiesDb.Any())
                {
                    var existedCities = cities.Where(c => existedCitiesDb.Values.Contains(c.Name)).ToList();

                    foreach (var city in existedCities)
                    {
                        var existedCityId = existedCitiesDb.First(e => e.Value == city.Name).Key;
                        EnrichCityWeatherInfo(city, existedCityId);
                    }

                    await _cityWeatherInfoRepository.AddMultipleAsync(existedCities.SelectMany(e => e.CityWeatherInfos)
                        .ToList());
                }

                var notExistedCities = existedCitiesDb.Any()
                    ? cities.Where(c => !existedCitiesDb.Values.Contains(c.Name)).ToList()
                    : cities;

                if (!notExistedCities.Any()) return true;

                foreach (var city in notExistedCities)
                {
                    var addedCityId = await _cityRepository.AddAsync(city);

                    if (addedCityId != CityParameters.AddCityErrorId)
                        EnrichCityWeatherInfo(city, addedCityId);
                }

                await _cityWeatherInfoRepository.AddMultipleAsync(notExistedCities.SelectMany(n => n.CityWeatherInfos)
                    .Where(n => n.CityId != CityParameters.AddCityErrorId)
                    .ToList());

                return true;
            }
            catch (Exception e)
            {
                _logger.LogError($"WeatherSiteDataSaveService SaveWeatherDataAsync error - [{e}]");
                return false;
            }
        }

        private static void EnrichCityWeatherInfo(DbCity city, int existedCityId)
        {
            foreach (var cityCityWeatherInfo in city.CityWeatherInfos)
            {
                cityCityWeatherInfo.CityId = existedCityId;
                cityCityWeatherInfo.CreatedAt = DateTime.UtcNow;
            }
        }
    }
}
