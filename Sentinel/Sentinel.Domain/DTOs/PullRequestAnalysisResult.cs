using Sentinel.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sentinel.Domain.DTOs
{
    public class PullRequestAnalysisResult
    {
        public List<FileAnalysisResult> Files { get; set; } = new();
    }
}
