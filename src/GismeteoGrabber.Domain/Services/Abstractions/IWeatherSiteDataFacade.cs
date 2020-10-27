using System.Threading;
using System.Threading.Tasks;

namespace GismeteoGrabber.Domain.Services.Abstractions
{
    /// <summary>
    /// Weather process facade
    /// </summary>
    public interface IWeatherSiteDataFacade
    {
        /// <summary>
        /// Process weather site data
        /// </summary>
        Task StartProcessSiteDataAsync(CancellationToken cancellationToken, string siteUrl);
    }
}
