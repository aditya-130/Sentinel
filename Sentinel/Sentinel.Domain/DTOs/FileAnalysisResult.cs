using Sentinel.Domain.DTOs;

namespace Sentinel.Domain.Entities
{
   
    public class FileAnalysisResult
    {
        public required string FilePath { get; set; }
        public List<MethodAnalysisResult> Methods { get; set; } = new();
        //public int IssueCount => Issues?.Count ?? 0;
    }
}
