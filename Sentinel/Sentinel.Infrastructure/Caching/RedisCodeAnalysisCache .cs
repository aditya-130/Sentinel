using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Sentinel.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Sentinel.Infrastructure.Caching
{
    public class RedisCodeAnalysisCache : ICodeAnalysisCache
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<RedisCodeAnalysisCache> _logger;
        private const string KeyPrefix = "sentinel:analysis:";

        public string Provider => "Redis";
        public RedisCodeAnalysisCache(IDistributedCache cache, ILogger<RedisCodeAnalysisCache> logger)
        {
            _cache = cache;
            _logger = logger;
        }

        public async Task<string?> GetAnalysisByCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            try
            {
                var codeHash = GenerateCodeHash(code);
                var cacheKey = GetCacheKey(codeHash);

                var cachedResult = await _cache.GetStringAsync(cacheKey, cancellationToken);

                if (cachedResult != null)
                {
                    _logger.LogInformation("Cache HIT for code hash: {CodeHash}", codeHash[..8]);
                }
                else
                {
                    _logger.LogInformation("Cache MISS for code hash: {CodeHash}", codeHash[..8]);
                }
                return cachedResult;
            }
            catch  (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving from cache. Proceeding without cache.");
                return null;
            }
          
        }

        public async Task SetAnalysisByCodeAsync(string code, string analysisResult, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var codeHash = GenerateCodeHash(code);
                var cacheKey = GetCacheKey(codeHash);
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiration ?? TimeSpan.FromHours(24)
                };
                await _cache.SetStringAsync(cacheKey, analysisResult, options, cancellationToken);
                _logger.LogInformation("Cached analysis for code hash: {CodeHash}", codeHash[..8]);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting cache. Continuing without caching.");
            }
        }
        private static string GenerateCodeHash(string code)
        {
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(code));
            return Convert.ToBase64String(hashBytes);
        }
        private static string GetCacheKey(string codeHash)
        {
            return $"{KeyPrefix}{codeHash}";
        }
    }
}
