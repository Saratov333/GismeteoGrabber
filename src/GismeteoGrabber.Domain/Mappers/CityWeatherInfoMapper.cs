using GismeteoGrabber.DAL.Models;
using GismeteoGrabber.Domain.Mappers.Abstractions;
using GismeteoGrabber.Domain.Models;

namespace GismeteoGrabber.Domain.Mappers
{
    /// <summary>
    /// CityWeatherInfo entity mapper
    /// </summary>
    public class CityWeatherInfoMapper : ICityWeatherInfoMapper
    {
        /// <summary>
        /// Map CityWeatherInfo entity
        /// </summary>
        public DbCityWeatherInfo Map(CityWeatherInfoSiteData cityWeatherInfoSiteData) =>
            new DbCityWeatherInfo(cityWeatherInfoSiteData.ForecastDateTime)
            {
                CityId = cityWeatherInfoSiteData.CityId,
                Precipitation = cityWeatherInfoSiteData.Precipitation,
                TemperatureFromCelsius = cityWeatherInfoSiteData.TemperatureFromCelsius,
                TemperatureToCelsius = cityWeatherInfoSiteData.TemperatureToCelsius,
                WindSpeed = cityWeatherInfoSiteData.WindSpeed
            };
    }
}
