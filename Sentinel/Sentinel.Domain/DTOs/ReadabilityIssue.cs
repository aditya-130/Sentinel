using Sentinel.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sentinel.Domain.Entities
{
    public class ReadabilityIssue
    {
        public required string Description { get; set; }

        public Severity Severity { get; set; }
        public string? Suggestion { get; set; }    
    }
}
