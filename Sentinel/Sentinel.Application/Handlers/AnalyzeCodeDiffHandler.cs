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
        private readonly ILanguageStrategyResolver _languageStrategyResolver;
        private readonly ILlmServiceResolver _llmServiceResolver;
        //private readonly ILanguageStrategy _languageStrategy;
        private readonly SchemaProvider _schemaProvider;
        private readonly PromptBuilder _promptBuilder;
        private readonly CodeAnalysisResponseParser _parser;


        public AnalyzeCodeDiffHandler(ILanguageStrategyResolver languageStrategyResolver, ILlmServiceResolver llmServiceResolver, SchemaProvider schemaProvider, PromptBuilder promptBuilder, CodeAnalysisResponseParser parser)
        {
            _languageStrategyResolver = languageStrategyResolver;
            _llmServiceResolver = llmServiceResolver;
            _schemaProvider = schemaProvider;
            _promptBuilder = promptBuilder;
            _parser = parser;
        }
        
        public async Task<AnalysisResult> Handle(CodeDiff codeDiff)
        {
            var languageStrategy = _languageStrategyResolver.Resolve(Language.CSharp);
            var codeChunks = languageStrategy.ExtractMethods(codeDiff.NewCode);
            var llmService = _llmServiceResolver.Resolve();
            var readabilityIssues = new List<ReadabilityIssue>();
            //var prompt = _promptBuilder.BuildPrompt(codeDiff.NewCode);
            var schema = _schemaProvider.GetCodeAnalysisSchema();
            foreach( var codeChunk in codeChunks)
            {
                var prompt = _promptBuilder.BuildPrompt(codeChunk.Code);
                var llmResponseJson = await llmService.AnalyzeCodeAsync(prompt, schema);
                var llmResult = _parser.Parse(llmResponseJson);
                readabilityIssues.AddRange(llmResult);
            }

            var dummyResultWithIssues = new AnalysisResult
            {
                FilePath = "",
                Issues = readabilityIssues
            };

            return dummyResultWithIssues;
        } 
    }
}
