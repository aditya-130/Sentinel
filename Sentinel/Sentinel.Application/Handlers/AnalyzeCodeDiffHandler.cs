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
        private readonly CodeAnalysisResponseParser __parser;

        public AnalyzeCodeDiffHandler(ILlmServiceResolver llmServiceResolver, SchemaProvider schemaProvider, PromptBuilder promptBuilder, CodeAnalysisResponseParser parser)
        {
            //_languageStrategy = languageStrategy;
            _llmServiceResolver = llmServiceResolver;
            _schemaProvider = schemaProvider;
            _promptBuilder = promptBuilder;
            __parser = parser;
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
            var llmResponseJson = await llmService.AnalyzeCodeAsync(prompt, schema);

            var readabiliityIssues = __parser.Parse(llmResponseJson);
            var dummyResultWithIssues = new AnalysisResult
            {
                FilePath = "",
                Issues = readabiliityIssues
            };

            return dummyResultWithIssues;
        } 
    }
}
