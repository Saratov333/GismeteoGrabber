using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GismeteoGrabber.DAL.Models;
using GismeteoGrabber.DAL.Repositories.Abstractions;
using GismeteoGrabber.Domain.Services;
using GismeteoGrabber.Domain.Services.Abstractions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;

namespace GismeteoGrabber.Tests
{
    [TestFixture]
    public class WeatherSiteDataSaveServiceTests
    {
        private IWeatherSiteDataSaveService _weatherSiteDataSaveService;
        private Mock<ICityRepository> _cityRepositoryMock;
        private Mock<ICityWeatherInfoRepository> _cityWeatherInfoRepositoryMock;
        private Mock<ILogger<WeatherSiteDataGrabService>> _loggerMock;

        private const string TestExistedCityName = "TestExistedCityName";

        [SetUp]
        public void SetUp()
        {
            _cityRepositoryMock = new Mock<ICityRepository>();
            _cityWeatherInfoRepositoryMock = new Mock<ICityWeatherInfoRepository>();
            _loggerMock = new Mock<ILogger<WeatherSiteDataGrabService>>();
            _weatherSiteDataSaveService = new WeatherSiteDataSaveService(_cityRepositoryMock.Object, _cityWeatherInfoRepositoryMock.Object, _loggerMock.Object);
        }

        [Test]
        public void Test_SaveWeatherDataAsync()
        {
            //arrange
            var testCities = new List<DbCity>
            {
                new DbCity
                {
                    Name = TestExistedCityName,
                    CityWeatherInfos = new List<DbCityWeatherInfo> {new DbCityWeatherInfo(DateTime.UtcNow)}
                },
                new DbCity
                {
                    CityWeatherInfos = new List<DbCityWeatherInfo>
                        {new DbCityWeatherInfo(DateTime.UtcNow), new DbCityWeatherInfo(DateTime.UtcNow.AddDays(1))}
                },
                new DbCity
                {
                    CityWeatherInfos = new List<DbCityWeatherInfo>
                    {
                        new DbCityWeatherInfo(DateTime.UtcNow), new DbCityWeatherInfo(DateTime.UtcNow.AddDays(1)),
                        new DbCityWeatherInfo(DateTime.UtcNow.AddDays(2))
                    }
                }
            };

            var testExistedCities = new Dictionary<int, string>
            {
                {1, TestExistedCityName}
            };

            var newTestCitiesCount = 2;
            var totalCitiesTypes = 2;

            _cityRepositoryMock.Setup(c => c.GetExistedCitiesAsync(It.IsAny<List<string>>())).Returns(Task.FromResult(testExistedCities));
            
            //act
            var result = _weatherSiteDataSaveService.SaveWeatherDataAsync(testCities).GetAwaiter().GetResult();

            //assert
            Assert.IsTrue(result);

            _cityRepositoryMock.Verify(a => a.AddAsync(It.IsAny<DbCity>()), Times.Exactly(newTestCitiesCount));
            _cityWeatherInfoRepositoryMock.Verify(a => a.AddMultipleAsync(It.IsAny<List<DbCityWeatherInfo>>()),
                Times.Exactly(totalCitiesTypes));
        }
    }
}
