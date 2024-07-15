using Chapter7_ContentFlagger.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Chapter7_ContentFlagger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentFlaggerController : ControllerBase
    {
        private readonly IAzureAiContentSafetyBusiness _azureAiContentSafetyBusiness;
        public ContentFlaggerController(IAzureAiContentSafetyBusiness azureAiContentSafetyBusines)
        {
            _azureAiContentSafetyBusiness = azureAiContentSafetyBusines;
        }
        [HttpGet]
        public IActionResult Get([FromQuery] string content)
        {
            var result = _azureAiContentSafetyBusiness.CheckForHateSpeech(content);

            return Ok(result);
        }
    }
}
