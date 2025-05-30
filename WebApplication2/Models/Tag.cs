namespace WebChronicles.Models
{
    public class Tag
    {

        public int Id { get; set; }

        public required string Title { get; set; }

        public string? Description { get; set; }
    }
}
