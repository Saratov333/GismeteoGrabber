using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace GismeteoGrabber.DAL.Models
{
    /// <summary>
    /// Main city data
    /// </summary>
    [Table("Cities")]
    public class DbCity
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Main data city name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Main data city url on the default site
        /// </summary>
        public string DataUrl { get; set; }

        /// <summary>
        /// Is city record active in DB
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// City weather info
        /// </summary>
        public IList<DbCityWeatherInfo> CityWeatherInfos { get; set; }
    }
}
