using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using GismeteoGrabber.Domain.Enums;
using GismeteoGrabber.Domain.Helpers;
using GismeteoGrabber.Domain.Models;
using GismeteoGrabber.Domain.Services.Abstractions;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace GismeteoGrabber.Domain.Services
{
    /// <summary>
    /// Weather grab service
    /// </summary>
    public class WeatherSiteDataGrabService : IWeatherSiteDataGrabService
    {
        private const int PopularCitiesForecastDays = 10;
        private const int PopularCityDefaultUnitTemperatureCount = 20;
        private const string TenDaysPrefix = "10-days";
        private const string PopularCitiesElementId = "noscript";

        private readonly ILogger<WeatherSiteDataGrabService> _logger;

        /// <summary>
        /// Weather grab service
        /// </summary>
        public WeatherSiteDataGrabService(ILogger<WeatherSiteDataGrabService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get site weather data
        /// </summary>
        public async Task<IList<CitySiteData>> GrabSiteWeatherByCitiesAsync(LoadHtmlSource loadHtmlSource, string dataSource)
        {
            if (string.IsNullOrWhiteSpace(dataSource))
                throw new Exception("Data source can't be empty");

            try
            {
                var mainHtmlDocument = await LoadHtmlDocumentDataAsync(loadHtmlSource, dataSource);
                var popularCities = GetPopularCitiesMainData(loadHtmlSource, mainHtmlDocument, dataSource);

                foreach (var popularCity in popularCities)
                {
                    var internalHtmlDocument = await LoadHtmlDocumentDataAsync(loadHtmlSource, popularCity.DataUrl); 
                    popularCity.CityWeatherInfos = GetPopularCityWeatherInfo(internalHtmlDocument, popularCity.DataUrl);
                }

                return popularCities;
            }
            catch (Exception e)
            {
                _logger.LogError($"WeatherSiteDataGrabService GrabSiteWeatherByCitiesAsync error - [{e}]");
                return new List<CitySiteData>();
            }
        }

        private async Task<HtmlDocument> LoadHtmlDocumentDataAsync(LoadHtmlSource loadHtmlSource, string dataSource)
        {
            switch (loadHtmlSource)
            {
                case LoadHtmlSource.Web:
                    return await LoadHtmlDocumentFromWebAsync(dataSource);
                case LoadHtmlSource.File:
                    return await LoadHtmlDocumentFromFileAsync(dataSource);
                default:
                    throw new Exception("Can't recognize load file source");
            }
        }

        private IList<CitySiteData> GetPopularCitiesMainData(LoadHtmlSource loadHtmlSource, HtmlDocument htmlDocument, string mainDataSiteUrl)
        {
                var popularCitiesElements = htmlDocument.GetElementbyId(PopularCitiesElementId);
                var popularCitiesCount = popularCitiesElements.SelectNodes("a").Count;
                var popularCities = CreatePopularCities(popularCitiesCount);

                var popularCityIndex = 0;

                foreach (HtmlNode popularCitiesElement in popularCitiesElements.SelectNodes("a"))
                {
                    var cityName = popularCitiesElement.GetAttributeValue("data-name", String.Empty);

                    if (string.IsNullOrWhiteSpace(cityName))
                        throw new Exception("City name can't be null or empty");

                    var cityDataUrl = popularCitiesElement.GetAttributeValue("data-url", String.Empty);

                    if (string.IsNullOrWhiteSpace(cityName))
                        throw new Exception("City data url can't be null or empty");

                    popularCities[popularCityIndex].Name = cityName;
                    popularCities[popularCityIndex].DataUrl = CreateDataUrl(loadHtmlSource, mainDataSiteUrl, cityDataUrl); ;
                    popularCities[popularCityIndex++].IsActive = true;
                }

                return popularCities;
        }

        private string CreateDataUrl(LoadHtmlSource loadHtmlSource, string mainDataSiteUrl, string cityDataUrl)
        {
            switch (loadHtmlSource)
            {
                case LoadHtmlSource.Web:
                    return mainDataSiteUrl + cityDataUrl + TenDaysPrefix;
                case LoadHtmlSource.File:
                    return Directory.GetCurrentDirectory() + @"\TestData\testCityWeatherInfoData.html";
                default:
                    throw new Exception("Can't recognize load source type");
            }
        }

        private IList<CityWeatherInfoSiteData> GetPopularCityWeatherInfo(HtmlDocument htmlDocument, string dataUrl)
        {
                var popularCitiesWeatherInfos = CreatePopularCityWeatherInfos();

                var unitTemperatures =
                    htmlDocument.DocumentNode.SelectNodes(
                        @"//div[@class='forecast_frame']//span[@class='unit unit_temperature_c']");

                if (unitTemperatures.Count != PopularCityDefaultUnitTemperatureCount)
                {
                    _logger.LogError("Wrong unitTemperatures.Count - dataUrl - {0}", dataUrl);   
                    return new List<CityWeatherInfoSiteData>();
                }

                var unitTemperatureIndex = 0;

                foreach (var cityWeatherInfo in popularCitiesWeatherInfos)
                {
                    cityWeatherInfo.TemperatureFromCelsius =
                        StringHelper.ConvertTemparatureToInt(unitTemperatures[unitTemperatureIndex++].InnerText);
                    cityWeatherInfo.TemperatureToCelsius =
                        StringHelper.ConvertTemparatureToInt(unitTemperatures[unitTemperatureIndex++].InnerText);
                }

                var unitWindSpeeds =
                    htmlDocument.DocumentNode.SelectNodes(
                        @"//div[@class='forecast_frame']//span[@class='unit unit_wind_m_s']");

                var windSpeedIndex = 0;

                foreach (var unitWindSpeed in unitWindSpeeds)
                {
                    popularCitiesWeatherInfos[windSpeedIndex++].WindSpeed = Convert.ToInt32(unitWindSpeed.InnerText);
                }

                var unitPrecipitations = htmlDocument.DocumentNode.SelectNodes(
                    @"//div[@class='forecast_frame']//div[@class='w_prec__value']");

                var unitPrecipitationIndex = 0;

                foreach (var unitPrecipitation in unitPrecipitations)
                {
                    popularCitiesWeatherInfos[unitPrecipitationIndex++].Precipitation =
                        StringHelper.FormatPrecipitation(unitPrecipitation.InnerText);
                }

                return popularCitiesWeatherInfos;
        }

        private async Task<HtmlDocument> LoadHtmlDocumentFromWebAsync(string dataUrl)
        {
            var htmlWeb = new HtmlWeb();

            return await htmlWeb.LoadFromWebAsync(dataUrl);
        }

        private async Task<HtmlDocument> LoadHtmlDocumentFromFileAsync(string dataSource)
        {
            var htmlDoc = new HtmlDocument();

            htmlDoc.Load(dataSource);

            return await Task.FromResult(htmlDoc);
        }

        private IList<CitySiteData> CreatePopularCities(int popularCitiesCount)
        {
            var popularCities = new List<CitySiteData>();

            for (int i = 0; i < popularCitiesCount; i++)
            {
                popularCities.Add(new CitySiteData());
            }

            return popularCities;
        }

        private IList<CityWeatherInfoSiteData> CreatePopularCityWeatherInfos()
        {
            var resultList = new List<CityWeatherInfoSiteData>(PopularCitiesForecastDays);
            var forecastDay = DateTime.UtcNow;

            for (int i = 0; i < PopularCitiesForecastDays; i++)
            {
                resultList.Add(new CityWeatherInfoSiteData(forecastDay));
                forecastDay = forecastDay.AddDays(1);
            }

            return resultList;
        }
    }
}
