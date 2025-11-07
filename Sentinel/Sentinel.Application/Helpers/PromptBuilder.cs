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

        For each issue you find, provide a clear description (e.g., 'ComplexMethod', 'BadNaming', 'NestingDepth'), the start line,
        the end line, severeity(Low, Mdeium, Hight or Critical) and a possible Suggestion.

        If the code is clear and has no issues, return an empty 'issues' array.
        Here is the code chunk:
        {0}
        """;

        public string BuildPrompt(string code)
        {

            return string.Format(ReadabilityAnalysisPromptCS, code);
        }
    }
}
