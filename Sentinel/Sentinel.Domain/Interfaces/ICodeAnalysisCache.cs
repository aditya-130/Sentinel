using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sentinel.Domain.Interfaces
{
    public interface ICodeAnalysisCache
    {
        string Provider { get; }
        Task<string?> GetAnalysisByCodeAsync(string code, CancellationToken cancellationToken = default);
        Task SetAnalysisByCodeAsync(string code, string analysisResult, TimeSpan? expiration = default, CancellationToken cancellationToken = default);
    }
}
