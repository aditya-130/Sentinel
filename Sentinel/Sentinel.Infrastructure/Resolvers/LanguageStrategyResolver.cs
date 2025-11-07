using Sentinel.Domain.Enums;
using Sentinel.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sentinel.Infrastructure.Resolvers
{
    public class LanguageStrategyResolver : ILanguageStrategyResolver
    {
        private readonly IEnumerable<ILanguageStrategy> _languageStrategies;
        public LanguageStrategyResolver(IEnumerable<ILanguageStrategy> languageStrategies)
        {
            _languageStrategies = languageStrategies;
        }
        public ILanguageStrategy Resolve(Language language)
        {
            var languageStrategy = _languageStrategies.FirstOrDefault(ls => ls.LanguageName == language);
            if (languageStrategy == null)
            {
                throw new InvalidOperationException("No Language Strategy registered for this language");
            }
            return languageStrategy;
        }
    }
}
