using System.Collections.Generic;
using System.Threading.Tasks;
using GismeteoGrabber.DAL.Models;

namespace GismeteoGrabber.Domain.Services.Abstractions
{
    /// <summary>
    /// Weather save service
    /// </summary>
    public interface IWeatherSiteDataSaveService
    {
        /// <summary>
        /// Save weather data to DB
        /// </summary>
        Task<bool> SaveWeatherDataAsync(List<DbCity> cities);
    }
}
