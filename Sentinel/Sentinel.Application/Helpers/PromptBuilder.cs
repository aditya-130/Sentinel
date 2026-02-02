using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sentinel.Application.Helpers
{
    public class PromptBuilder
    {
        private const string ReadabilityAnalysisPromptCS = """
        You are an expert code reviewer analyzing C# code for readability issues.
        Your task is to analyze the following code and identify all significant
        readability problems.

        READABILITY ISSUES TO CHECK FOR:
        - Methods longer than 50 lines
        - More than 3 levels of nesting (if/for/while)
        - Complex boolean conditions (3+ conditions combined)
        - Chaining 4+ LINQ operations on one line
        - Variable names that don't explain their purpose e.g., 'x', 'temp', 'data' (Only flag naming issues when the name is unclear in context or could reasonably confuse a reader.
        Do not flag standard math-like names such as i, j, x, y when used in tiny/obvious operations (e.g., Add(a,b)), unless the method is non-trivial)
        - Missing comments for complex, non-obvious logic
        - Too many parameters (5+)

        CRITICAL: Do not mention line numbers

        For each issue you find, provide:
        - description: Clear issue type (e.g., 'ComplexMethod', 'BadNaming', 'NestingDepth')
        - severity: Low, Medium, High, or Critical
        - suggestion: Specific suggestion on how to fix it

        If the code is clear and has no issues, return an empty 'issues' array.

        CODE CHUNK TO ANALYZE:
        {0}
        """;

        public string BuildPrompt(string code)
        {
            return string.Format(ReadabilityAnalysisPromptCS, code);
        }
    }
}