using Chapter7_ContentFlagger.Model;

namespace Chapter7_ContentFlagger.Business
{
    public interface IAzureAiContentSafetyBusiness
    {
        public ResponseDto CheckForHateSpeech(string content);
    }
}
