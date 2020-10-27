using System.Collections.Generic;
using System.Threading.Tasks;
using GismeteoGrabber.DAL.Models;

namespace GismeteoGrabber.DAL.Repositories.Abstractions
{
    /// <summary>
    /// City DB repository
    /// </summary>
    public interface ICityRepository
    {
        /// <summary>
        /// Add new city to DB
        /// </summary>
        Task<int> AddAsync(DbCity city);

        /// <summary>
        /// Get existed cities ids by names
        /// </summary>
        Task<Dictionary<int, string>> GetExistedCitiesAsync(List<string> cityNames);
    }
}
