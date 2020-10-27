using System.Linq;
using GismeteoGrabber.DAL.Models;
using GismeteoGrabber.Domain.Mappers.Abstractions;
using GismeteoGrabber.Domain.Models;

namespace GismeteoGrabber.Domain.Mappers
{
    /// <summary>
    /// City entity mapper
    /// </summary>
    public class CityMapper : ICityMapper
    {
        private readonly ICityWeatherInfoMapper _cityWeatherInfoMapper;

        public CityMapper(ICityWeatherInfoMapper cityWeatherInfoMapper)
        {
            _cityWeatherInfoMapper = cityWeatherInfoMapper;
        }

        /// <summary>
        /// Map city entity
        /// </summary>
        public DbCity Map(CitySiteData citySiteData)
        {
            return new DbCity
            {
                Name = citySiteData.Name,
                DataUrl = citySiteData.DataUrl,
                IsActive = citySiteData.IsActive,
                CityWeatherInfos = citySiteData.CityWeatherInfos.Select(c => _cityWeatherInfoMapper.Map(c)).ToList()
            };
        }
    }
}
