namespace Chapter7_ContentFlagger.Model
{
    public class ResponseDto
    {
        public int ServerityLevel { get; set; }
        public bool NeedsHumanSupervision { get; set; }
        public bool ShouldBeFlagged { get; set; }
    }
}
