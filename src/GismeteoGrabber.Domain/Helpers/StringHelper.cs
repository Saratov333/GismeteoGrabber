using System;
using System.Text.RegularExpressions;
using System.Threading;
using NLog;

namespace GismeteoGrabber.Domain.Helpers
{
    /// <summary>
    /// String helper
    /// </summary>
    public static class StringHelper
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static readonly string CurrentSystemDecimalSeparator =
            Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator;

        /// <summary>
        /// Format precipitation to decimal
        /// </summary>
        public static decimal FormatPrecipitation(string unitPrecipitationInnerText)
        {
            var precipitationFormatted = unitPrecipitationInnerText.Replace(".", CurrentSystemDecimalSeparator)
                .Replace(",", CurrentSystemDecimalSeparator).Trim();

            try
            {
                return Convert.ToDecimal(precipitationFormatted);
            }
            catch (Exception e)
            {
                Logger.Error(e, $"StringHelper FormatPrecipitation error");
                throw;
            }
        }

        /// <summary>
        /// Convert temperature value to int
        /// </summary>
        public static int ConvertTemparatureToInt(string innerText)
        {
            try
            {
                if (!innerText.Contains("minus"))
                {
                    return Convert.ToInt32(innerText);
                }

                var onlyDigitsRegex = new Regex(@"[0-9]{1,3}");

                if (onlyDigitsRegex.IsMatch(innerText))
                {
                    return -Convert.ToInt32(onlyDigitsRegex.Match(innerText).Value);
                }

                throw new Exception("Can't recognize number");
            }
            catch (Exception e)
            {
                Logger.Error(e, $"StringHelper ConvertTemparatureToInt error");
                throw;
            }
        }
    }
}
