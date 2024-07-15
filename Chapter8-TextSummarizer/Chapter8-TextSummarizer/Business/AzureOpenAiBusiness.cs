using Azure;
using Azure.AI.OpenAI;

namespace Chapter8_TextSummarizer.Business
{
    public class AzureOpenAiBusiness : IAzureOpenAiBusiness
    {
        private readonly IConfiguration _configuration;
        private static OpenAIClient openAIClient;
        public AzureOpenAiBusiness(IConfiguration configuration)
        {

            _configuration = configuration;
            openAIClient = new OpenAIClient(new Uri(_configuration["AzureOpenAiEndpoint"]), 
                new AzureKeyCredential(_configuration["AzureOpenAiKey"]));

        }
        public async Task<string> GetSummaryAsync(string content)
        {
            var chatCompletionOptions = new ChatCompletionsOptions
            {
                MaxTokens = 400,
                Temperature = 1f,
                FrequencyPenalty = 0.0f,
                PresencePenalty = 0.0f,
                NucleusSamplingFactor = 0.95f,
                DeploymentName = _configuration["AzureOpenAiDeploymentName"]
            };
            string userPrompt = "Please summarize the the following article in 60 words or less:\n" + content;
            chatCompletionOptions.Messages.Add(new ChatRequestUserMessage(userPrompt));

            ChatCompletions response = await openAIClient.GetChatCompletionsAsync(chatCompletionOptions);
            ChatResponseMessage assistantResponse = response.Choices[0].Message;

            return assistantResponse.Content;
        }

        
    }
}
