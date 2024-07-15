namespace Chapter8_TextSummarizer.Business
{
    public interface IAzureOpenAiBusiness
    {
        public Task<string> GetSummaryAsync(string content);
    }
}
