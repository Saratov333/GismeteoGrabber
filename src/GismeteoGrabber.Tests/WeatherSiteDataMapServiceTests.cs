using System;
using System.Collections.Generic;
using GismeteoGrabber.DAL.Models;
using GismeteoGrabber.Domain.Mappers.Abstractions;
using GismeteoGrabber.Domain.Models;
using GismeteoGrabber.Tests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace GismeteoGrabber.Tests
{
    [TestFixture]
    public class WeatherSiteDataMapServiceTests
    {
        private ICityMapper _cityMapper;
        private const int FirstTestCityId = 1;
        private const int SecondTestCityId = 2;
        private const string FirstTestCityName = "TestCity1";
        private const string SecondTestCityName = "TestCity2";
        private const string FirstTestCityDataUrl = "TestCity1DataUrl";
        private const string SecondTestCityDataUrl = "TestCity2DataUrl";
        private const bool FirstTestCityActive = true;
        private const bool SecondTestCityActive = false;
        private readonly DateTime _firstTestCityForecasDateTime = new DateTime(2019,10,10);
        private readonly DateTime _secondTestCityForecasDateTime = new DateTime(2018, 11, 11);
        private const int FirstTestCityTemperatureToCelsius = 1;
        private const int SecondTestCityTemperatureToCelsius = 2;
        private const int FirstTestCityTemperatureFromCelsius = 0;
        private const int SecondTestCityTemperatureFromCelsius = -1;
        private const int FirstTestCityWindSpeed = 2;
        private const int SecondTestCityWindSpeed = 5;
        private const decimal FirstTestCityPrecipitation = 0.2m;
        private const decimal SecondTestCityPrecipitation = 1.6m;

        [SetUp]
        public void SetUp()
        {
            var testHost = TestHelper.GetTestHost();
            _cityMapper = testHost.Services.GetService<ICityMapper>();
        }

        [Test]
        public void Test_Map()
        {
            //arrange
            var testCitiesSiteData = GetTestCitySiteDatas();
            var resultCitiesData = new List<DbCity>();

            //act
            foreach (var testCitySiteData in testCitiesSiteData)
            {
                resultCitiesData.Add(_cityMapper.Map(testCitySiteData));
            }

            //assert
            Assert.AreEqual(resultCitiesData[0].Name, FirstTestCityName);
            Assert.AreEqual(resultCitiesData[0].DataUrl, FirstTestCityDataUrl);
            Assert.AreEqual(resultCitiesData[0].IsActive, FirstTestCityActive);
            Assert.AreEqual(resultCitiesData[0].CityWeatherInfos[0].CityId, FirstTestCityId);
            Assert.AreEqual(resultCitiesData[0].CityWeatherInfos[0].ForecastDateTime, _firstTestCityForecasDateTime);
            Assert.AreEqual(resultCitiesData[0].CityWeatherInfos[0].TemperatureFromCelsius, FirstTestCityTemperatureFromCelsius);
            Assert.AreEqual(resultCitiesData[0].CityWeatherInfos[0].TemperatureToCelsius, FirstTestCityTemperatureToCelsius);
            Assert.AreEqual(resultCitiesData[0].CityWeatherInfos[0].WindSpeed, FirstTestCityWindSpeed);
            Assert.AreEqual(resultCitiesData[0].CityWeatherInfos[0].Precipitation, FirstTestCityPrecipitation);

            Assert.AreEqual(resultCitiesData[1].Name, SecondTestCityName);
            Assert.AreEqual(resultCitiesData[1].DataUrl, SecondTestCityDataUrl);
            Assert.AreEqual(resultCitiesData[1].IsActive, SecondTestCityActive);
            Assert.AreEqual(resultCitiesData[1].CityWeatherInfos[0].CityId, SecondTestCityId);
            Assert.AreEqual(resultCitiesData[1].CityWeatherInfos[0].ForecastDateTime, _secondTestCityForecasDateTime);
            Assert.AreEqual(resultCitiesData[1].CityWeatherInfos[0].TemperatureFromCelsius, SecondTestCityTemperatureFromCelsius);
            Assert.AreEqual(resultCitiesData[1].CityWeatherInfos[0].TemperatureToCelsius, SecondTestCityTemperatureToCelsius);
            Assert.AreEqual(resultCitiesData[1].CityWeatherInfos[0].WindSpeed, SecondTestCityWindSpeed);
            Assert.AreEqual(resultCitiesData[1].CityWeatherInfos[0].Precipitation, SecondTestCityPrecipitation);
        }

        private IList<CitySiteData> GetTestCitySiteDatas()
        {
            var testCityWeatherInfos = GetCityWeatherInfoSiteDatas();

            return new List<CitySiteData>
            {
                new CitySiteData
                {
                    Name = FirstTestCityName, DataUrl = FirstTestCityDataUrl, IsActive = FirstTestCityActive,
                    CityWeatherInfos = new List<CityWeatherInfoSiteData>{testCityWeatherInfos[0]}
                },
                new CitySiteData
                {
                    Name = SecondTestCityName, DataUrl = SecondTestCityDataUrl, IsActive = SecondTestCityActive,
                    CityWeatherInfos = new List<CityWeatherInfoSiteData>{testCityWeatherInfos[1]}
                }
            };
        }

        private IList<CityWeatherInfoSiteData> GetCityWeatherInfoSiteDatas()
        {
            return new List<CityWeatherInfoSiteData>
            {
                new CityWeatherInfoSiteData
                {
                    CityId = FirstTestCityId, ForecastDateTime = _firstTestCityForecasDateTime, TemperatureToCelsius = FirstTestCityTemperatureToCelsius, TemperatureFromCelsius = FirstTestCityTemperatureFromCelsius,
                    WindSpeed = FirstTestCityWindSpeed, Precipitation = FirstTestCityPrecipitation
                },
                new CityWeatherInfoSiteData
                {
                    CityId = SecondTestCityId, ForecastDateTime = _secondTestCityForecasDateTime, TemperatureToCelsius = SecondTestCityTemperatureToCelsius, TemperatureFromCelsius = SecondTestCityTemperatureFromCelsius,
                    WindSpeed = SecondTestCityWindSpeed, Precipitation = SecondTestCityPrecipitation
                }
            };
        }
    }
}
