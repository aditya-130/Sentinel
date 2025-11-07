using Sentinel.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sentinel.Domain.Entities
{
   
    public class AnalysisResult
    {
        public required string FilePath { get; set; }
        public List<ReadabilityIssue> Issues { get; set; } = new List<ReadabilityIssue>();
        public int IssueCount => Issues?.Count ?? 0;
    }
}
