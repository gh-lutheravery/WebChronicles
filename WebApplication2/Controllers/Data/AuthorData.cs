using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using WebApplication2.Models;
using Microsoft.Extensions.Configuration;

namespace WebApplication2.Controllers.Data
{
    public class AuthorData
    {
        private readonly string _connectionString;

        public AuthorData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public int CreateAuthor(Author author)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("INSERT INTO Authors (Name, Bio, Title, Avatar, Email, Password, Joined) OUTPUT INSERTED.Id VALUES (@Name, @Bio, @Title, @Avatar," +
                "                            @Email, @Password, @Joined)", conn);
            cmd.Parameters.AddWithValue("@Name", author.Name ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Bio", author.Bio ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Title", author.Title ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Avatar", author.Avatar ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Email", author.Email ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Password", author.Password ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Joined", author.Joined);

            conn.Open();
            return (int)cmd.ExecuteScalar();
        }

        public Author? GetAuthorById(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT Id, Name, Bio FROM Authors WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Author
                {
                    Id = reader.GetInt32(0),
                    Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                    Bio = reader.IsDBNull(2) ? null : reader.GetString(2)
                };
            }
            return null;
        }

        public List<Author> GetAllAuthors()
        {
            var authors = new List<Author>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("SELECT Id, Name, Bio, Title, Joined FROM Authors", conn);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                authors.Add(new Author
                {
                    Id = reader.GetInt32(0),
                    Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                    Bio = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Title = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Joined = reader.GetDateTime(4),
                });
            }
            return authors;
        }
    }
}
