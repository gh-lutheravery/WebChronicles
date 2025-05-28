namespace WebChronicles.Models
{
    public class Chapter
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime Posted { get; set; }

        public string Content { get; set; }

        public int StoryId { get; set; }

        public Story Story { get; set; }
    }
}
