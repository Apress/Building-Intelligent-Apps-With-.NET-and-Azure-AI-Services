using Chapter3_LanguageTranslator.Business;
using Chapter3_LanguageTranslator.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Chapter3_LanguageTranslator
{
    public class Translator
    {
        private readonly ILogger<Translator> _logger;
        private readonly ITranslatorBusiness _translatorBusiness;

        public Translator(ILogger<Translator> logger, ITranslatorBusiness translatorBusiness)
        {
            _logger = logger;
            _translatorBusiness = translatorBusiness;
        }

        [Function("Translator")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

                var payload = JsonConvert.DeserializeObject<Payload>(requestBody);

                _logger.LogInformation("C# HTTP trigger function processed a request.");

                var result = await _translatorBusiness.TranslateMessageAsync(payload.Message, payload.TargetLanguage);

                _logger.LogInformation($"Translation: {result}");
                return new OkObjectResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new BadRequestObjectResult(ex.Message);

            }
        }
    }
}
