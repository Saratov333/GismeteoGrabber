using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using GismeteoGrabber.DAL.Constants;
using GismeteoGrabber.DAL.Models;
using GismeteoGrabber.DAL.Options;
using GismeteoGrabber.DAL.Repositories.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GismeteoGrabber.DAL.Repositories
{
    /// <summary>
    /// Cities repository
    /// </summary>
    public class CityRepository : ICityRepository
    {
        private readonly ILogger<CityRepository> _logger;
        private readonly string _connectionString;
        private const bool DefaultActiveCityValue = true;

        /// <summary>
        /// Cities repository
        /// </summary>
        public CityRepository(
            IOptions<GismeteoGrabberConnectOptions> options,
            ILogger<CityRepository> logger)
        {
            _logger = logger;
            _connectionString = options.Value.ConnectionString;
        }    

        /// <summary>
        /// Add new city
        /// </summary>
        public async Task<int> AddAsync(DbCity city)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    var insertCityQuery =
                        "INSERT INTO Cities(Name, DataUrl, IsActive, CreatedAt) OUTPUT INSERTED.[Id] VALUES (@Name, @DataUrl, @IsActive, @CreatedAt)";

                    return await connection.QuerySingleAsync<int>(insertCityQuery,
                        new { city.Name, city.DataUrl, city.IsActive, CreatedAt = DateTime.UtcNow });
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"CityRepository AddAsync error - [{e}]");
                return CityParameters.AddCityErrorId;
            }
        }

        /// <summary>
        /// Get existed cities ids by names
        /// </summary>
        public async Task<Dictionary<int, string>> GetExistedCitiesAsync(List<string> cityNames)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(_connectionString))
                {
                    var existedCitiesQuery =
                        "SELECT Id, Name FROM Cities WHERE Name IN @CityNames AND IsActive = @IsActive";

                    var result = await connection.QueryAsync<DbCity>(existedCitiesQuery,
                        new { CityNames = cityNames, IsActive = DefaultActiveCityValue });

                    return result.ToDictionary(x => x.Id, x => x.Name);
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"CityRepository GetExistedCitiesAsync error - [{e}]");
                return new Dictionary<int, string>();
            }
        }
    }
}
