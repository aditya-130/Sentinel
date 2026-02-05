using Microsoft.Extensions.Configuration;
using Sentinel.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sentinel.Infrastructure.Resolvers
{
    public class CacheServiceResolver : ICacheServiceResolver
    {
        private readonly IEnumerable<ICodeAnalysisCache> _cacheServices;
        private readonly IConfiguration _configuration;
        public CacheServiceResolver(IEnumerable<ICodeAnalysisCache> cacheServices, IConfiguration configuration)
        {
            _cacheServices = cacheServices;
            _configuration = configuration;
        }
        public ICodeAnalysisCache Resolve()
        {
            var cacheProvider = _configuration["CacheProvider"];
            var service = _cacheServices.FirstOrDefault(s => s.Provider == cacheProvider);

            if (service == null)
            {
                throw new InvalidOperationException($"No cache service registered for provider: {cacheProvider}");
            }

            return service;
        }
    }
}
