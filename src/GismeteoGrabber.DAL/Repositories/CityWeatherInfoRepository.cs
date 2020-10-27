using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper.Contrib.Extensions;
using GismeteoGrabber.DAL.Models;
using GismeteoGrabber.DAL.Options;
using GismeteoGrabber.DAL.Repositories.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GismeteoGrabber.DAL.Repositories
{
    /// <summary>
    /// City weather info's repository
    /// </summary>
    public class CityWeatherInfoRepository : ICityWeatherInfoRepository
    {
        private readonly ILogger<CityWeatherInfoRepository> _logger;
        private readonly string _connectionString;

        /// <summary>
        /// City weather info's repository
        /// </summary>
        public CityWeatherInfoRepository(
            IOptions<GismeteoGrabberConnectOptions> options,
            ILogger<CityWeatherInfoRepository> logger)
        {
            _logger = logger;
            _connectionString = options.Value.ConnectionString;
        }

        /// <summary>
        /// Add new city's weather infos collection
        /// </summary>
        public async Task<bool> AddMultipleAsync(List<DbCityWeatherInfo> cityWeatherInfos)
        {
            using (IDbConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    return await connection.InsertAsync(cityWeatherInfos) > 0;
                }
                catch (Exception e)
                {
                    _logger.LogError($"CityWeatherInfoRepository AddMultipleAsync error - [{e}]");
                    return false;
                }
            }
        }
    }
}
