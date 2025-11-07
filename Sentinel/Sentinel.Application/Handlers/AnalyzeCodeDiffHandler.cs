using Sentinel.Application.Helpers;
using Sentinel.Domain.Entities;
using Sentinel.Domain.Enums;
using Sentinel.Domain.Interfaces;

namespace Sentinel.Application.Handlers
{
    public class AnalyzeCodeDiffHandler
    {
        private readonly ILanguageStrategyResolver _languageStrategyResolver;
        private readonly ILlmServiceResolver _llmServiceResolver;
        private readonly SchemaProvider _schemaProvider;
        private readonly PromptBuilder _promptBuilder;
        private readonly CodeAnalysisResponseParser _parser;

        public AnalyzeCodeDiffHandler(
            ILanguageStrategyResolver languageStrategyResolver,
            ILlmServiceResolver llmServiceResolver,
            SchemaProvider schemaProvider,
            PromptBuilder promptBuilder,
            CodeAnalysisResponseParser parser)
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
            var schema = _schemaProvider.GetCodeAnalysisSchema();

            var allIssues = new List<ReadabilityIssue>();
            const int batchSize = 5;

            for (int i = 0; i < codeChunks.Count; i += batchSize)
            {
                var batch = codeChunks.Skip(i).Take(batchSize);

                var tasks = batch.Select(async chunk =>
                {
                    try
                    {
                        var prompt = _promptBuilder.BuildPrompt(chunk.Code);
                        var response = await llmService.AnalyzeCodeAsync(prompt, schema);
                        return _parser.Parse(response);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"ERROR analyzing method '{chunk.MethodName}': {ex.Message}");
                        return new List<ReadabilityIssue>();

                    }
                });

                var batchResults = await Task.WhenAll(tasks);
                allIssues.AddRange(batchResults.SelectMany(issues => issues));
            }

            return new AnalysisResult
            {
                FilePath = codeDiff.FilePath,
                Issues = allIssues
            };
        }
    }
}