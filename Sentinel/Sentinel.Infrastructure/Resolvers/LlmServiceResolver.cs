using Microsoft.Extensions.Configuration;
using Sentinel.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sentinel.Infrastructure.Resolvers
{
    public class LlmServiceResolver : ILlmServiceResolver
    {
        private readonly IEnumerable<ILlmService> _llmServices;
        private readonly IConfiguration _configuration;
        public LlmServiceResolver(IEnumerable<ILlmService> llmServices, IConfiguration configuration)
        {
            _llmServices = llmServices;
            _configuration = configuration;
        }
        public ILlmService Resolve()
        {
            var llmProvider = _configuration["LlmServiceProvider"] ;
            var service =  _llmServices.FirstOrDefault(service => service.Provider == llmProvider);
            if (service == null)
            {
                throw new InvalidOperationException("No LLM Sercice registered for provider");
            }
            return service;
        }
    }
}
