namespace CenzurazoApi.Models
{
    public class CensorResponse
    {
        public string ModifiedText { get; set; } = string.Empty;
        public List<WordFrequency> OriginalFrequencies { get; set; } = new();
        public List<WordFrequency> ModifiedFrequencies { get; set; } = new();
    }

    public class WordFrequency
    {
        public string Word { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
