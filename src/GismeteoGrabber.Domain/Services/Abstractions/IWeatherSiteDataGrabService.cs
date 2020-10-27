using System.Collections.Generic;
using System.Threading.Tasks;
using GismeteoGrabber.Domain.Enums;
using GismeteoGrabber.Domain.Models;

namespace GismeteoGrabber.Domain.Services.Abstractions
{
    /// <summary>
    /// Weather grab service
    /// </summary>
    public interface IWeatherSiteDataGrabService
    {
        /// <summary>
        /// Get site weather data
        /// </summary>
        Task<IList<CitySiteData>> GrabSiteWeatherByCitiesAsync(LoadHtmlSource loadHtmlSource, string dataSource);
    }
}
