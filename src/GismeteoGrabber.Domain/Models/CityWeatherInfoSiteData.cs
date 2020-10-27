using System;

namespace GismeteoGrabber.Domain.Models
{
    /// <summary>
    /// Additional city weather info
    /// </summary>
    public class CityWeatherInfoSiteData
    {
        /// <summary>
        /// Main city weather info id
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// Date of forecast grab
        /// </summary>
        public DateTime ForecastDateTime { get; set; }

        /// <summary>
        /// Current forecast temperature from in celsius
        /// </summary>
        public int TemperatureFromCelsius { get; set; }

        /// <summary>
        /// Current forecast temperature to in celsius
        /// </summary>
        public int TemperatureToCelsius { get; set; }

        /// <summary>
        /// Current forecast wind speed
        /// </summary>
        public int WindSpeed { get; set; }

        /// <summary>
        /// Current forecast precipitation
        /// </summary>
        public decimal Precipitation { get; set; }

        /// <summary>
        /// Additional city weather info
        /// </summary>
        public CityWeatherInfoSiteData()
        {
            
        }

        /// <summary>
        /// Additional city weather info
        /// </summary>
        public CityWeatherInfoSiteData(DateTime forecastDateTime)
        {
            ForecastDateTime = forecastDateTime;
        }
    }
}
