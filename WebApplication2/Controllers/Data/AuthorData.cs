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
            using var cmd = new SqlCommand("INSERT INTO Authors (Name, Bio) OUTPUT INSERTED.Id VALUES (@Name, @Bio)", conn);
            cmd.Parameters.AddWithValue("@Name", author.Name ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Bio", author.Bio ?? (object)DBNull.Value);

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
            using var cmd = new SqlCommand("SELECT Id, Name, Bio FROM Authors", conn);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                authors.Add(new Author
                {
                    Id = reader.GetInt32(0),
                    Name = reader.IsDBNull(1) ? null : reader.GetString(1),
                    Bio = reader.IsDBNull(2) ? null : reader.GetString(2)
                });
            }
            return authors;
        }

        public bool UpdateAuthor(Author author)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("UPDATE Authors SET Name = @Name, Bio = @Bio WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", author.Id);
            cmd.Parameters.AddWithValue("@Name", author.Name ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Bio", author.Bio ?? (object)DBNull.Value);

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }

        public bool DeleteAuthor(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand("DELETE FROM Authors WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);

            conn.Open();
            return cmd.ExecuteNonQuery() > 0;
        }
    }
}
