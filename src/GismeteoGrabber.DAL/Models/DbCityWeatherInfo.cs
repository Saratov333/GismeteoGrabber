using System;
using Dapper.Contrib.Extensions;

namespace GismeteoGrabber.DAL.Models
{
    /// <summary>
    /// City weather info
    /// </summary>
    [Table("CityWeatherInfos")]
    public class DbCityWeatherInfo
    {
        /// <summary>
        /// Main city id
        /// </summary>
        public int CityId { get; set; }

        /// <summary>
        /// Date of forecast grab
        /// </summary>
        public DateTime ForecastDateTime { get; set; }

        /// <summary>
        /// Current forecast temperature from, in celsius
        /// </summary>
        public int TemperatureFromCelsius { get; set; }

        /// <summary>
        /// Current forecast temperature to, in celsius
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
        /// Date of the forecast creation
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// City weather info
        /// </summary>
        public DbCityWeatherInfo(DateTime forecastDateTime)
        {
            ForecastDateTime = forecastDateTime;
        }
    }
}
