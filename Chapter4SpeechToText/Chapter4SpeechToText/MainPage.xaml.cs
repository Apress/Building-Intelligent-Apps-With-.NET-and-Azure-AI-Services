using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech;

namespace Chapter4SpeechToText
{
    public partial class MainPage : ContentPage
    {
       
        public MainPage()
        {
            InitializeComponent();
        }

        

        private async void btnTranscribe_Clicked(object sender, EventArgs e)
        {
            var absolutePathToWavFile = entAbsolutePath.Text;
            var speechKey = entSubscriptionKey.Text;
            var speechRegion = entRegion.Text;

            var speechConfig = SpeechConfig.FromSubscription(speechKey, speechRegion);
            speechConfig.SpeechRecognitionLanguage = "en-US";

            using var audioConfig = AudioConfig.FromWavFileInput(absolutePathToWavFile);

            using var speechRecognizer = new SpeechRecognizer(speechConfig, audioConfig);

            var speechRecognitionResult = await speechRecognizer.RecognizeOnceAsync();

            var result = OutputSpeechRecognitionResult(speechRecognitionResult);

            lblTranscribeResult.Text = $"Transcribed text - {result}";
        }
        static string OutputSpeechRecognitionResult(SpeechRecognitionResult speechRecognitionResult)
        {
            string result = string.Empty;
            switch (speechRecognitionResult.Reason)
            {
                case ResultReason.RecognizedSpeech:
                    result = speechRecognitionResult.Text;
                    break;
                case ResultReason.NoMatch:
                    result = "NOMATCH: Speech could not be recognized.";
                    break;
                case ResultReason.Canceled:
                    
                    result = $"CANCELED: Operation cancelled";
                    break;
            }
            return result;
        }
    }

}
