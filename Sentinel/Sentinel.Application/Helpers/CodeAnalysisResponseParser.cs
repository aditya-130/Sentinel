using Sentinel.Domain.Entities;
using Sentinel.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sentinel.Application.Helpers
{
    public class CodeAnalysisResponseParser
    {
        public List<ReadabilityIssue> Parse(string jsonResponse)
        {
            using var document = JsonDocument.Parse(jsonResponse);
            var issueArray = document.RootElement.GetProperty("issues").EnumerateArray();
            var readabilityIssues = new List<ReadabilityIssue>();
            foreach (var issue in issueArray)
            {
                readabilityIssues.Add(new ReadabilityIssue
                {
                    Description = issue.GetProperty("description").GetString()!,
                    StartLine = issue.GetProperty("startLine").GetInt32(),
                    EndLine = issue.GetProperty("endLine").GetInt32(),
                    Severity = ParseSeverity(issue.GetProperty("severity").GetString()!),
                    Suggestion = issue.TryGetProperty("suggestion", out var suggestion) ? suggestion.GetString() : null,
                });
            }
            return readabilityIssues;
        }
        
        private Severity ParseSeverity(string severityString)
        {
            return severityString switch
            {
                "Critical" => Severity.Critical,
                "High" => Severity.High,
                "Medium" => Severity.Medium,
                "Low" => Severity.Low,
                _ => Severity.Low,
            };
        }
    }
}
