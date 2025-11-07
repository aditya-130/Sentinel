using Sentinel.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sentinel.Domain.Interfaces
{
    public interface ILanguageStrategyResolver
    {
        ILanguageStrategy Resolve(Language language);
    }
}
