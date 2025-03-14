namespace DeveSpotnet.Models
{
    public class SpotPost
    {
        public long ArticleNumber { get; set; }
        public string Subject { get; set; }
        public string MessageId { get; set; }
        // You can expand this model to include additional metadata (e.g. description, poster, etc.)
    }
}
