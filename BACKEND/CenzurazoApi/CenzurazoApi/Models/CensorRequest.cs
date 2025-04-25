namespace CenzurazoApi.Models
{
    public class CensorRequest
    {
        public string BlacklistText { get; set; } = string.Empty;
        public string InputText { get; set; } = string.Empty;
    }
}
