using Chapter8_TextSummarizer.Business;
using Chapter8_TextSummarizer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chapter8_TextSummarizer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SummarizeController : ControllerBase
    {
        private readonly IAzureOpenAiBusiness _azureOpenAiBusiness;
        public SummarizeController(IAzureOpenAiBusiness azureOpenAiBusiness)
        {
            _azureOpenAiBusiness = azureOpenAiBusiness;
        }
        [HttpPost(Name = "GetSummary")]
        public async Task<IActionResult> GetSummary([FromBody] Payload requestPayload)
        {
            if (string.IsNullOrEmpty(requestPayload.ArticleData))
            {
                return BadRequest("Article data cannot be empty.");
            }

            var summary = await _azureOpenAiBusiness.GetSummaryAsync(requestPayload.ArticleData);

            return Ok(summary);

        }
    }
}
