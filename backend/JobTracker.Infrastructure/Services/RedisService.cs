using JobTracker.Application.Interfaces;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace JobTracker.Infrastructure.Services
{
    public class RedisService : ICacheService
    {
        private readonly IDatabase _db;
        private readonly ILogger<RedisService> _logger;

        public RedisService(IConnectionMultiplexer redis, ILogger<RedisService> logger)
        {
            _db = redis.GetDatabase();
            _logger = logger;
        }

        public async Task<string?> GetAsync(string key)
        {
            try
            {
                var value = await _db.StringGetAsync(key);
                return value.HasValue ? value.ToString() : null;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Redis GET failed for key: {Key}", key);
                return null;
            }
        }

        public async Task RemoveAsync(string key)
        {
            try
            {
                await _db.KeyDeleteAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Redis REMOVE failed for key: {Key}", key);
            }
        }

        public async Task SetAsync(string key, string value, TimeSpan expiry)
        {
            try
            {
                await _db.StringSetAsync(key, value, expiry);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Redis SET failed for key: {Key}", key);
            }
        }
    }
}
