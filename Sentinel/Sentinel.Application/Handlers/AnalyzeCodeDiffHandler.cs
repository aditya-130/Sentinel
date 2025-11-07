using Sentinel.Application.Helpers;
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
        private readonly ILlmServiceResolver _llmServiceResolver;
        //private readonly ILanguageStrategy _languageStrategy;
        private readonly SchemaProvider _schemaProvider;
        private readonly PromptBuilder _promptBuilder;

        public AnalyzeCodeDiffHandler(ILlmServiceResolver llmServiceResolver, SchemaProvider schemaProvider, PromptBuilder promptBuilder)
        {
            //_languageStrategy = languageStrategy;
            _llmServiceResolver = llmServiceResolver;
            _schemaProvider = schemaProvider;
            _promptBuilder = promptBuilder;
        }
        
        public async Task<AnalysisResult> Handle(CodeDiff codeDiff)
        {
            var llmService = _llmServiceResolver.Resolve();
            //var codeChunks = _languageStrategy.ExtractMethods(codeDiff.NewCode);
            var prompt = _promptBuilder.BuildPrompt(codeDiff.NewCode);
            var schema = _schemaProvider.GetCodeAnalysisSchema();
            //foreach (var codeChunk in codeChunks)
            //{
            //    var llmResponse = await llmService.AnalyzeCodeAsync(prompt, schema);

            //}
            var llmResponse = await llmService.AnalyzeCodeAsync(prompt, schema);

            var dummyIssues = new List<ReadabilityIssue>
            {
                new ReadabilityIssue
                {
                    Description = "desc",
                    StartLine = 1,
                    EndLine = 2,
                    Severity = Severity.Low,
                    Suggestion = llmResponse
                }
            };
            var dummyResultWithIssues = new AnalysisResult
            {
                FilePath = "",
                Issues = dummyIssues
            };

            return dummyResultWithIssues;
        } 
    }
}
