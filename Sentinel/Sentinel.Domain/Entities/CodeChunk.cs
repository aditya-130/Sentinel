using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sentinel.Domain.Entities
{
    //This class does violate Open-Closed principle as future extention to include class level and property level analysis would require modification.
    //Keeping the class simple for now to prevent over engineering.
    //Future Work: Can consider using inheritance (MethodChunk, ClassChunk, PropertyChunk etc..)
    public class CodeChunk
    {
        public required string Code { get; set; }
        public required int StartLine { get; set; }
        public required int EndLine { get; set; }
        public required string MethodName { get; set; }
        public CodeChunk(string code, int startLine, int endLine, string methodName)
        {
            Code = code;
            StartLine = startLine;
            EndLine = endLine;
            MethodName = methodName;
        }
    }
}
