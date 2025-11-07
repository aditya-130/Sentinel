using Microsoft.AspNetCore.Mvc;
using Sentinel.Application.Handlers;
using Sentinel.Domain.Entities;

namespace Sentinel.Api.Controllers
{
    [ApiController]
    [Route("ap/[controller]")]
    public class AnalyzeCodeController : ControllerBase
    {
        private readonly AnalyzeCodeDiffHandler _analyzeCodeDiffHandler;
        public AnalyzeCodeController(AnalyzeCodeDiffHandler analyzeCodeDiffHandler)
        {
            _analyzeCodeDiffHandler = analyzeCodeDiffHandler;
        }

        [HttpPost("analyze")]
        public async Task<ActionResult<string>> Analyze([FromBody] CodeDiff codeDiff)
        {
            var readabilityIssues =  await _analyzeCodeDiffHandler.Handle(codeDiff);
            return Ok(readabilityIssues);
        }

    }
}
