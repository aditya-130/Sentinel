using Microsoft.AspNetCore.Mvc;
using Sentinel.Application.Handlers;
using Sentinel.Domain.Entities;

namespace Sentinel.Api.Controllers
{
    [ApiController]
    [Route("apI/[controller]")]
    public class AnalyzePullRequestController : ControllerBase
    {
        private readonly AnalyzeCodeDiffHandler _analyzeCodeDiffHandler;
        public AnalyzePullRequestController(AnalyzeCodeDiffHandler analyzeCodeDiffHandler)
        {
            _analyzeCodeDiffHandler = analyzeCodeDiffHandler;
        }

        [HttpPost("analyze")]
        public async Task<ActionResult<string>> Analyze([FromBody] List<CodeDiff> codeDiffs)
        {
            var readabilityIssues =  await _analyzeCodeDiffHandler.Handle(codeDiffs);
            return Ok(readabilityIssues);
        }

    }
}
