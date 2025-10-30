using Sentinel.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sentinel.Domain.Entities
{
    public class ReadabilityIssue
    {
        public required string Description { get; set; }
        public int StartLine { get; set; }
        public int EndLine { get; set; }
        public Severity Severity { get; set; }
        public string? Suggestion { get; set; }
        public ReadabilityIssue(string description, int startLine, int endLine, Severity severity, string? suggestion)
        {
            Description = description;
            StartLine = startLine;
            EndLine = endLine;
            Severity = severity;
            Suggestion = suggestion;
        }
    }
}
