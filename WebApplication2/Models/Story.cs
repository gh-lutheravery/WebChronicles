namespace WebChronicles.Models
{
    using System;
    using System.Collections.Generic;

    public class Story
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public string Title { get; set; }

        public string? Image { get; set; }

        public string Status { get; set; }

        public int AuthorId { get; set; }

        public string? Description { get; set; }

        public DateTime Posted { get; set; }

        public int Followers { get; set; }

        public int Favorites { get; set; }

        public int Views { get; set; }


        public Author? Author { get; set; }

        public ICollection<Chapter>? Chapters { get; set; }
        public ICollection<Tag>? Tags { get; set; }
    }
}
