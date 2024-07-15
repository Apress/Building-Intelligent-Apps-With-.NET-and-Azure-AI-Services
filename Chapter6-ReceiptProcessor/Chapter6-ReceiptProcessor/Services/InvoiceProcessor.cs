using Azure;
using Azure.AI.FormRecognizer.DocumentAnalysis;
using Chapter6_ReceiptProcessor.Model;
using Microsoft.Extensions.Configuration;

namespace Chapter6_ReceiptProcessor.Services
{
    public class InvoiceProcessor : IInvoiceProcessor
    {
        private readonly IConfiguration _configuration;
        private DocumentAnalysisClient client;
        public InvoiceProcessor(IConfiguration configuration)
        {
            _configuration = configuration;
            string endpoint = _configuration["DocumentIntelligenceEndpoint"];
            string apiKey = _configuration["DocumentIntelligenceKey"];
            var credential = new AzureKeyCredential(apiKey);
            client = new DocumentAnalysisClient(new Uri(endpoint), credential);
        }
        public Invoice GetInvoice(Stream fileContent)
        {
            AnalyzeDocumentOperation operation = client.AnalyzeDocument(waitUntil: WaitUntil.Completed, "prebuilt-document", fileContent);
            AnalyzeResult result = operation.Value;

            Invoice invoice = new Invoice();

            foreach (DocumentKeyValuePair kvp in result.KeyValuePairs)
            {
                if (kvp.Value == null)
                {
                    Console.WriteLine($"  Found key with no value: '{kvp.Key.Content}'");
                }
                else
                {
                    if (kvp.Key.Content.Contains("Invoice Number"))
                    {
                        invoice.InvoiceNumber = kvp.Value.Content;
                        continue;
                    }
                    if (kvp.Key.Content.Contains("Invoice Date"))
                    {
                        invoice.InvoiceDate = kvp.Value.Content;
                        continue;
                    }
                    if (kvp.Key.Content.Contains("GSTIN"))
                    {
                        invoice.Gstin = kvp.Value.Content;
                        continue;
                    }
                    if (kvp.Key.Content.Contains("Invoice Amount"))
                    {
                        invoice.InvoiceAmount = kvp.Value.Content;
                        continue;
                    }
                    if (kvp.Key.Content.Contains("GST"))
                    {
                        invoice.GstAmount = kvp.Value.Content;
                        continue;
                    }
                    if (kvp.Key.Content.Contains("Total"))
                    {
                        invoice.TotalAmount = kvp.Value.Content;
                        continue;
                    }
                }
            }           

            return invoice;

        }
    }
}
