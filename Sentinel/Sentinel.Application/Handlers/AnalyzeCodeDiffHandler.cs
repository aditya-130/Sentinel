using Microsoft.Extensions.Logging;
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
        private readonly ICacheServiceResolver _cacheServiceResolver;
        private readonly ILogger<AnalyzeCodeDiffHandler> _logger;
        private readonly SchemaProvider _schemaProvider;
        private readonly PromptBuilder _promptBuilder;
        private readonly CodeAnalysisResponseParser _parser;

        public AnalyzeCodeDiffHandler(
            ILanguageStrategyResolver languageStrategyResolver,
            ILlmServiceResolver llmServiceResolver,
            ICacheServiceResolver cacheServiceResolver,
            ILogger<AnalyzeCodeDiffHandler> logger,
            SchemaProvider schemaProvider,
            PromptBuilder promptBuilder,
            CodeAnalysisResponseParser parser)
        {
            _languageStrategyResolver = languageStrategyResolver;
            _llmServiceResolver = llmServiceResolver;
            _cacheServiceResolver = cacheServiceResolver;
            _logger = logger;
            _schemaProvider = schemaProvider;
            _promptBuilder = promptBuilder;
            _parser = parser;
        }

        public async Task<PullRequestAnalysisResult> Handle(List<CodeDiff> codeDiffs)
        {
            var result = new PullRequestAnalysisResult();

            var languageStrategy = _languageStrategyResolver.Resolve(Language.CSharp);
            var llmService = _llmServiceResolver.Resolve();
            var cacheService = _cacheServiceResolver.Resolve();
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
                            var cachedResponse = await cacheService.GetAnalysisByCodeAsync(chunk.Code);

                            string response;
                            if (cachedResponse != null)
                            {
                                response = cachedResponse;
                            }
                            else
                            {
                                var prompt = _promptBuilder.BuildPrompt(chunk.Code);
                                response = await llmService.AnalyzeCodeAsync(prompt, schema);
                                await cacheService.SetAnalysisByCodeAsync(chunk.Code, response);
                            }

                            var issues = _parser.Parse(response);

                            return new MethodAnalysisResult
                            {
                                MethodName = chunk.MethodName,
                                Issues = issues
                            };
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error analyzing method '{MethodName}' in '{FilePath}'",chunk.MethodName, codeDiff.FilePath);
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
