using Azure;
using Azure.AI.ContentSafety;
using Chapter7_ContentFlagger.Model;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using static System.Net.Mime.MediaTypeNames;

namespace Chapter7_ContentFlagger.Business
{
    public class AzureAiContentSafetyBusiness : IAzureAiContentSafetyBusiness
    {
        private readonly IConfiguration _configuration;
        private static ContentSafetyClient client;
        public AzureAiContentSafetyBusiness(IConfiguration configuration)
        {

            _configuration = configuration;
            client = new ContentSafetyClient(new Uri(_configuration["AzureAiContentSafetyEndpoint"]), 
                new AzureKeyCredential(_configuration["AzureAiContentSafetyKey"]));

        }
        public ResponseDto CheckForHateSpeech(string content)
        {
            var request = new AnalyzeTextOptions(content);

            var response = client.AnalyzeText(request);

            var serverityLevel = response.Value.CategoriesAnalysis.FirstOrDefault(a => a.Category == TextCategory.Hate)?.Severity ?? 0;

            ResponseDto responseDto = new ResponseDto();

            if(serverityLevel < 4) { 
                responseDto.ServerityLevel = serverityLevel;
                responseDto.ShouldBeFlagged = false;
                responseDto.NeedsHumanSupervision = false;
            }
            if (serverityLevel > 3)
            {
                responseDto.ServerityLevel = serverityLevel;
                responseDto.ShouldBeFlagged = true;
                responseDto.NeedsHumanSupervision = true;
            }
            return responseDto;
        }
    }
}
