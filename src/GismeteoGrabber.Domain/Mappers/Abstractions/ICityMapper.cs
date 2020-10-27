using GismeteoGrabber.DAL.Models;
using GismeteoGrabber.Domain.Models;

namespace GismeteoGrabber.Domain.Mappers.Abstractions
{
    /// <summary>
    /// City entity mapper
    /// </summary>
    public interface ICityMapper
    {
        /// <summary>
        /// Map city entity
        /// </summary>
        DbCity Map(CitySiteData citySiteData);
    }
}
