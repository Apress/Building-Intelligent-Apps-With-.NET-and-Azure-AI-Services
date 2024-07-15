using System.IO;
using System.Threading.Tasks;
using Azure.AI.TextAnalytics;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Azure.Storage.Blobs;  
using System.Text;

namespace Chapter2DocumentClassifier
{
    public class DocumentClassifier
    {
        private readonly ILogger<DocumentClassifier> _logger;
        private readonly TextAnalyticsClient _textAnalyticsClient;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly BlobContainerClient _blobSourceContainerClient;
        private readonly BlobContainerClient _blobDestinationContainerClient;
        public DocumentClassifier(ILogger<DocumentClassifier> logger, TextAnalyticsClient textAnalyticsClient, BlobServiceClient blobServiceClient)
        {
            _logger = logger;
            _textAnalyticsClient = textAnalyticsClient;
            _blobServiceClient = blobServiceClient;
            _blobSourceContainerClient = _blobServiceClient.GetBlobContainerClient(Environment.GetEnvironmentVariable("sourceContainerName"));
            _blobDestinationContainerClient = _blobServiceClient.GetBlobContainerClient(Environment.GetEnvironmentVariable("targetContainerName"));
        }

        [Function(nameof(DocumentClassifier))]
        public async Task Run([BlobTrigger("source/{name}", Source = BlobTriggerSource.LogsAndContainerScan, Connection = "blobConn")] Stream stream, string name)
        {
            using var blobStreamReader = new StreamReader(stream);
            var content = await blobStreamReader.ReadToEndAsync();
            _logger.LogInformation($"C# Blob Trigger processed blob\n Name: {name} \n Data: {content}");

            var result = await _textAnalyticsClient.DetectLanguageAsync(content);

            string detectedLanguage = result.Value.Name;

            _logger.LogInformation($"Detected language: {detectedLanguage}");

            string targetBlobName = $"{detectedLanguage}/{name}";

            BlobClient blobClient = _blobDestinationContainerClient.GetBlobClient(targetBlobName);

            byte[] byteArray = Encoding.UTF8.GetBytes(content);
            await blobClient.UploadAsync(new MemoryStream(byteArray));

            _logger.LogInformation($"Upload blob - {targetBlobName} in the {Environment.GetEnvironmentVariable("targetContainerName")} container.");

            await _blobSourceContainerClient.DeleteBlobIfExistsAsync(name);

            _logger.LogInformation($"Deleted blob - {name} from {Environment.GetEnvironmentVariable("sourceContainerName")} container.");


        }
    }
}
