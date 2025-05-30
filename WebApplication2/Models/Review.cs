namespace WebChronicles.Models
{
    public class Review
    {
        public int Id { get; set; }

        public int OverallScore { get; set; }

        public int GrammarScore { get; set; }

        public int AuthorId { get; set; }

        public Author? Author { get; set; }

        public required string Content { get; set; }

        public DateTime Posted { get; set; }
    }
}
