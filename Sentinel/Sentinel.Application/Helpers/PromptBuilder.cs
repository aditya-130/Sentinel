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
        - Variable names that don't explain their purpose (e.g., 'x', 'temp', 'data')
        - Missing comments for complex, non-obvious logic
        - Too many parameters (5+)

        CRITICAL: When reporting line numbers, count from the FIRST LINE of the code chunk below as line 1.
        The first line of code you see is line 1, the second line is line 2, and so on.
        Do NOT count any lines from this prompt - ONLY count lines in the code chunk.

        For each issue you find, provide:
        - description: Clear issue type (e.g., 'ComplexMethod', 'BadNaming', 'NestingDepth')
        - startLine: Line number where the issue starts (counting from 1 in the code chunk below)
        - endLine: Line number where the issue ends (counting from 1 in the code chunk below)
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