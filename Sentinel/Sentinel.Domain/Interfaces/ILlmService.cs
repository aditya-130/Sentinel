using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sentinel.Domain.Interfaces
{
    public interface ILlmService
    {
        string Provider { get; }
        Task<string> AnalyzeCodeAsync(string prompt, string schema);
    }
}
