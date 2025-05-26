using Microsoft.Data.SqlClient;

namespace WebApplication2.Controllers.Data
{
    public class DataInit
    {
        public static void InsertAuthors(SqlConnection connection)
        {
            for (int i = 1; i <= 10; i++)
            {
                var command = new SqlCommand(@"
                INSERT INTO Authors (Name, Title, Avatar, Email, Password, Bio, Joined)
                VALUES (@Name, @Title, @Avatar, @Email, @Password, @Bio, @Joined)", connection);

                command.Parameters.AddWithValue("@Name", $"Author {i}");
                command.Parameters.AddWithValue("@Title", $"Writer Rank {i}");
                command.Parameters.AddWithValue("@Avatar", $"https://avatars.example.com/{i}.png");
                command.Parameters.AddWithValue("@Email", $"author{i}@example.com");
                command.Parameters.AddWithValue("@Password", $"password{i}");
                command.Parameters.AddWithValue("@Bio", $"This is the bio of author {i}");
                command.Parameters.AddWithValue("@Joined", DateTime.Today.AddDays(-i * 10));

                command.ExecuteNonQuery();
            }
        }

        public static void InsertStories(SqlConnection connection)
        {
            for (int i = 1; i <= 10; i++)
            {
                var command = new SqlCommand(@"
                INSERT INTO Stories (Type, Title, Image, Status, AuthorId, Description, Posted, Followers, Favorites, Views)
                VALUES (@Type, @Title, @Image, @Status, @AuthorId, @Description, @Posted, @Followers, @Favorites, @Views)", connection);

                command.Parameters.AddWithValue("@Type", "Fantasy");
                command.Parameters.AddWithValue("@Title", $"Epic Tale {i}");
                command.Parameters.AddWithValue("@Image", $"https://images.example.com/story{i}.jpg");
                command.Parameters.AddWithValue("@Status", i % 2 == 0 ? "Ongoing" : "Completed");
                command.Parameters.AddWithValue("@AuthorId", (i % 10) + 1); // Ensure 1–10
                command.Parameters.AddWithValue("@Description", $"This is the description of story {i}");
                command.Parameters.AddWithValue("@Posted", DateTime.Today.AddDays(-i));
                command.Parameters.AddWithValue("@Followers", 100 + i);
                command.Parameters.AddWithValue("@Favorites", 50 + i);
                command.Parameters.AddWithValue("@Views", 1000 + i * 10);

                command.ExecuteNonQuery();
            }
        }
    }
}
