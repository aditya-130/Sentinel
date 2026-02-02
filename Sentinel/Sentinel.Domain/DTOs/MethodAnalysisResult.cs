using Sentinel.Domain.Entities;

namespace Sentinel.Domain.DTOs
{
    public class MethodAnalysisResult
    {
        public required string  MethodName { get; set; }
        public List<ReadabilityIssue> Issues { get; set; } = new List<ReadabilityIssue>();
    }
}
