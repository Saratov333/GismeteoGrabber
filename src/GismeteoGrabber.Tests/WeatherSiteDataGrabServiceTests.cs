using System;
using System.Linq;
using GismeteoGrabber.Domain.Enums;
using GismeteoGrabber.Domain.Services.Abstractions;
using GismeteoGrabber.Tests.Helpers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace GismeteoGrabber.Tests
{
    [TestFixture]
    public class WeatherSiteDataGrabServiceTests
    {
        private IWeatherSiteDataGrabService _weatherSiteDataGrabService;
        private IConfiguration _testConfiguration;

        private const int TestCityTemperatureFromCelsius = 0;
        private const int TestCityTemperatureToCelsius = -8;
        private const int TestCityWindSpeed = 10;
        private const decimal TestCityPrecipitation = 2.6m;
        private const int DefaultForecastDaysExceptToday = 9;

        [SetUp]
        public void SetUp()
        {
            var testHost = TestHelper.GetTestHost();
            _weatherSiteDataGrabService = testHost.Services.GetService<IWeatherSiteDataGrabService>();
            _testConfiguration = testHost.Services.GetService<IConfiguration>();
        }

        [Test]
        public void Test_GrabSiteWeatherByCitiesAsync()
        {
            //arrange
            var path = _testConfiguration.GetValue<string>("DefaultMainSiteDataPath");

            //act
            var result = _weatherSiteDataGrabService.GrabSiteWeatherByCitiesAsync(LoadHtmlSource.File, path).GetAwaiter().GetResult();

            //assert
            Assert.IsNotEmpty(result);

            Assert.AreEqual(result[0].CityWeatherInfos[0].TemperatureFromCelsius, TestCityTemperatureFromCelsius);
            Assert.AreEqual(result[0].CityWeatherInfos[0].TemperatureToCelsius, TestCityTemperatureToCelsius);
            Assert.AreEqual(result[0].CityWeatherInfos[0].WindSpeed, TestCityWindSpeed);
            Assert.AreEqual(result[0].CityWeatherInfos[0].Precipitation, TestCityPrecipitation);

            Assert.AreEqual(result[0].CityWeatherInfos[0].ForecastDateTime.Date, DateTime.UtcNow.Date);
            Assert.AreEqual(result[0].CityWeatherInfos.Last().ForecastDateTime.Date, DateTime.UtcNow.AddDays(DefaultForecastDaysExceptToday).Date);
        }
    }
}
