using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sentinel.Domain.Entities
{
    public class CodeDiff
    {
        public required string FilePath { get; set; }
        public string? OldCode { get; set; }
        public required  string NewCode { get; set; }
    }
}
