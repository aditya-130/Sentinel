using Sentinel.Application.Helpers;
using Sentinel.Domain.DTOs;
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

        public async Task<PullRequestAnalysisResult> Handle(List<CodeDiff> codeDiffs)
        {
            var result = new PullRequestAnalysisResult();

            var languageStrategy = _languageStrategyResolver.Resolve(Language.CSharp);
            var llmService = _llmServiceResolver.Resolve();
            var schema = _schemaProvider.GetCodeAnalysisSchema();

            const int batchSize = 5;

            foreach (var codeDiff in codeDiffs)
            {
                var codeChunks = languageStrategy.ExtractMethods(codeDiff.NewCode);

                var methods = new List<MethodAnalysisResult>();

                for (int i = 0; i < codeChunks.Count; i += batchSize)
                {
                    var batch = codeChunks.Skip(i).Take(batchSize).ToList();

                    var tasks = batch.Select(async chunk =>
                    {
                        try
                        {
                            var prompt = _promptBuilder.BuildPrompt(chunk.Code);
                            var response = await llmService.AnalyzeCodeAsync(prompt, schema);
                            var issues = _parser.Parse(response);

                            return new MethodAnalysisResult
                            {
                                MethodName = chunk.MethodName,
                                Issues = issues
                            };
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"ERROR analyzing method '{chunk.MethodName}' in '{codeDiff.FilePath}': {ex.Message}");

                            return new MethodAnalysisResult
                            {
                                MethodName = chunk.MethodName,
                                Issues = new List<ReadabilityIssue>()
                            };
                        }
                    });

                    var batchResults = await Task.WhenAll(tasks);
                    methods.AddRange(batchResults);
                }

                result.Files.Add(new FileAnalysisResult
                {
                    FilePath = codeDiff.FilePath,
                    Methods = methods
                });
            }

            return result;
        }
    }
}
