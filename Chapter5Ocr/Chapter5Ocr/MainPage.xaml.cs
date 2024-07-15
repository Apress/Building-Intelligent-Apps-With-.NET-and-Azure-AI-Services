using Azure.AI.Vision.ImageAnalysis;
using Azure;
using System.Net;

namespace Chapter5Ocr
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }


        private void btnExtractText_Clicked(object sender, EventArgs e)
        {
            var absolutePathToImageFile = entAbsolutePath.Text;
            var visionKey = entSubscriptionKey.Text;
            var visionEndpoint = entEndpoint.Text;

            ImageAnalysisClient client = new ImageAnalysisClient(
                new Uri(visionEndpoint),
                new AzureKeyCredential(visionKey));

            using FileStream stream = new FileStream(absolutePathToImageFile, FileMode.Open);
            BinaryData imageData = BinaryData.FromStream(stream);

            ImageAnalysisResult result = client.Analyze(imageData, VisualFeatures.Read);

            string extractedText = String.Empty;
            foreach (DetectedTextBlock block in result.Read.Blocks)
                foreach (DetectedTextLine line in block.Lines)
                {
                    extractedText = extractedText + "\n" + line.Text;
                }

            lblExtractedTextResult.Text = extractedText;
        }
    }

}
