using System.Collections.Generic;

namespace GismeteoGrabber.Domain.Models
{
    /// <summary>
    /// Main city data
    /// </summary>
    public class CitySiteData
    {
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
        /// Additional city weather info
        /// </summary>
        public IList<CityWeatherInfoSiteData> CityWeatherInfos { get; set; }
    }
}
