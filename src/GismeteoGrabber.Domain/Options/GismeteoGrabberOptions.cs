namespace GismeteoGrabber.Domain.Options
{
    /// <summary>
    /// Main grabber options
    /// </summary>
    public class GismeteoGrabberOptions
    {
        /// <summary>
        /// Grab site url
        /// </summary>
        public string DefaultSiteDataUrl { get; set; }

        /// <summary>
        /// Grab interval
        /// </summary>
        public int DefaultDelayTimeMinutes { get; set; }
    }
}
