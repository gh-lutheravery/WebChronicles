namespace WebApplication2.Models
{
    public class Author
    {

        public int Id { get; set; }


        public string Name { get; set; }


        public string Title { get; set; }


        public string Avatar { get; set; }


        public string Email { get; set; }


        public string Password { get; set; }


        public string Bio { get; set; }

        public DateTime Joined { get; set; }

        public ICollection<Story> Stories { get; set; } = new List<Story>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
