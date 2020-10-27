using GismeteoGrabber.DAL.Models;
using GismeteoGrabber.Domain.Models;

namespace GismeteoGrabber.Domain.Mappers.Abstractions
{
    /// <summary>
    /// CityWeatherInfo entity mapper
    /// </summary>
    public interface ICityWeatherInfoMapper
    {
        /// <summary>
        /// Map CityWeatherInfo entity
        /// </summary>
        DbCityWeatherInfo Map(CityWeatherInfoSiteData cityWeatherInfoSiteData);
    }
}
