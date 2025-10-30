using Sentinel.Domain.Entities;
using Sentinel.Domain.Enums;
using Sentinel.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sentinel.Application.Handlers
{
    public class AnalyzeCodeDiffHandler
    {
        private readonly ILlmService _llmService;
        private readonly ILanguageStrategy _languageStrategy;

        public AnalyzeCodeDiffHandler(ILlmService llmService, ILanguageStrategy languageStrategy)
        {
            _languageStrategy = languageStrategy;
            _llmService = llmService;
        }
        
        public async Task<AnalysisResult> Handle(CodeDiff codeDiff)
        {
            var codeChunks = _languageStrategy.ExtractMethods(codeDiff.NewCode);
            var prompt = "This is a dummy prompt";
            foreach (var codeChunk in codeChunks)
            {
                var llmResponse = await _llmService.AnalyzeCodeAsync(codeChunk.Code, prompt);

            }
           
            var dummyIssues = new List<ReadabilityIssue>
            {
                new ReadabilityIssue( "description", 1, 2, Severity.Low, "suggestion")
            };
            var dummyResultWithIssues = new AnalysisResult("filepath", dummyIssues);
            return dummyResultWithIssues;
        } 
    }
}
