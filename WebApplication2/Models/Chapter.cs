namespace WebChronicles.Models
{
    public class Chapter
    {
        public int Id { get; set; }

        public required string Title { get; set; }

        public DateTime Posted { get; set; }

        public required string Content { get; set; }

        public int StoryId { get; set; }

        public Story? Story { get; set; }
    }
}
