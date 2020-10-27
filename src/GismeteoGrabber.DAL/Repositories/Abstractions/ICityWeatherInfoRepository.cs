using System.Collections.Generic;
using System.Threading.Tasks;
using GismeteoGrabber.DAL.Models;

namespace GismeteoGrabber.DAL.Repositories.Abstractions
{
    /// <summary>
    /// City weather info's repository
    /// </summary>
    public interface ICityWeatherInfoRepository
    {
        /// <summary>
        /// Add new city's weather infos collection
        /// </summary>
        Task<bool> AddMultipleAsync(List<DbCityWeatherInfo> cityWeatherInfos);
    }
}
