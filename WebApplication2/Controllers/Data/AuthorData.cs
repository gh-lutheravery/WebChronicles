using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebChronicles.Models;

namespace WebChronicles.Controllers.Data
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
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string commandString = @"
                    INSERT INTO Authors (Name, Bio, Title, Avatar, Email, Password, Joined) 
                    OUTPUT INSERTED.Id 
                    VALUES (@Name, @Bio, @Title, @Avatar, @Email, @Password, @Joined)";

                using (var cmd = new SqlCommand(commandString, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", (object?)author.Name ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Bio", (object?)author.Bio ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Title", (object?)author.Title ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Avatar", (object?)author.Avatar ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", (object?)author.Email ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Password", (object?)author.Password ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Joined", author.Joined);

                    var authorId = (int)cmd.ExecuteScalar();
                    return authorId;
                }
            }
        }

        public Author? GetAuthorById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string commandString = @"
                    SELECT Id, Name, Bio, Title, Avatar, Email, Password, Joined 
                    FROM Authors 
                    WHERE Id = @Id";

                using (var cmd = new SqlCommand(commandString, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        int idOrdinal = reader.GetOrdinal("Id");
                        int nameOrdinal = reader.GetOrdinal("Name");
                        int bioOrdinal = reader.GetOrdinal("Bio");
                        int titleOrdinal = reader.GetOrdinal("Title");
                        int avatarOrdinal = reader.GetOrdinal("Avatar");
                        int emailOrdinal = reader.GetOrdinal("Email");
                        int passwordOrdinal = reader.GetOrdinal("Password");
                        int joinedOrdinal = reader.GetOrdinal("Joined");

                        if (reader.Read())
                        {
                            return new Author
                            {
                                Id = reader.GetInt32(idOrdinal),
                                Name = reader.GetString(nameOrdinal),
                                Bio = reader.IsDBNull(bioOrdinal) ? null : reader.GetString(bioOrdinal),
                                Title = reader.IsDBNull(titleOrdinal) ? null : reader.GetString(titleOrdinal),
                                Avatar = reader.IsDBNull(avatarOrdinal) ? null : reader.GetString(avatarOrdinal),
                                Email = reader.GetString(emailOrdinal),
                                Password = reader.GetString(passwordOrdinal),
                                Joined = reader.GetDateTime(joinedOrdinal)
                            };
                        }
                    }
                }
                return null;
            }
        }

        public Author? GetAuthorByEmail(string email)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string commandString = @"
                    SELECT Id, Name, Bio, Title, Avatar, Email, Password, Joined 
                    FROM Authors 
                    WHERE Email = @Email";

                using (var cmd = new SqlCommand(commandString, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);

                    using (var reader = cmd.ExecuteReader())
                    {
                        int idOrdinal = reader.GetOrdinal("Id");
                        int nameOrdinal = reader.GetOrdinal("Name");
                        int bioOrdinal = reader.GetOrdinal("Bio");
                        int titleOrdinal = reader.GetOrdinal("Title");
                        int avatarOrdinal = reader.GetOrdinal("Avatar");
                        int emailOrdinal = reader.GetOrdinal("Email");
                        int passwordOrdinal = reader.GetOrdinal("Password");
                        int joinedOrdinal = reader.GetOrdinal("Joined");

                        if (reader.Read())
                        {
                            return new Author
                            {
                                Id = reader.GetInt32(idOrdinal),
                                Name = reader.GetString(nameOrdinal),
                                Bio = reader.IsDBNull(bioOrdinal) ? null : reader.GetString(bioOrdinal),
                                Title = reader.IsDBNull(titleOrdinal) ? null : reader.GetString(titleOrdinal),
                                Avatar = reader.IsDBNull(avatarOrdinal) ? null : reader.GetString(avatarOrdinal),
                                Email = reader.GetString(emailOrdinal),
                                Password = reader.GetString(passwordOrdinal),
                                Joined = reader.GetDateTime(joinedOrdinal)
                            };
                        }
                    }
                }
                return null;
            }
        }

        public List<Author> GetAllAuthors()
        {
            var authors = new List<Author>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string commandString = @"
                    SELECT Id, Name, Bio, Title, Avatar, Email, Password, Joined 
                    FROM Authors";

                using (var cmd = new SqlCommand(commandString, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        int idOrdinal = reader.GetOrdinal("Id");
                        int nameOrdinal = reader.GetOrdinal("Name");
                        int bioOrdinal = reader.GetOrdinal("Bio");
                        int titleOrdinal = reader.GetOrdinal("Title");
                        int avatarOrdinal = reader.GetOrdinal("Avatar");
                        int emailOrdinal = reader.GetOrdinal("Email");
                        int passwordOrdinal = reader.GetOrdinal("Password");
                        int joinedOrdinal = reader.GetOrdinal("Joined");

                        while (reader.Read())
                        {
                            authors.Add(new Author
                            {
                                Id = reader.GetInt32(idOrdinal),
                                Name = reader.GetString(nameOrdinal),
                                Bio = reader.IsDBNull(bioOrdinal) ? null : reader.GetString(bioOrdinal),
                                Title = reader.IsDBNull(titleOrdinal) ? null : reader.GetString(titleOrdinal),
                                Avatar = reader.IsDBNull(avatarOrdinal) ? null : reader.GetString(avatarOrdinal),
                                Email = reader.GetString(emailOrdinal),
                                Password = reader.GetString(passwordOrdinal),
                                Joined = reader.GetDateTime(joinedOrdinal)
                            });
                        }
                    }
                }
            }
            return authors;
        }

        public bool UpdateAuthor(Author author)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string commandString = @"
                    UPDATE Authors 
                    SET Name = @Name,
                        Bio = @Bio,
                        Title = @Title,
                        Avatar = @Avatar,
                        Email = @Email
                    WHERE Id = @Id";

                using (var cmd = new SqlCommand(commandString, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", author.Name);
                    cmd.Parameters.AddWithValue("@Bio", (object?)author.Bio ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Title", (object?)author.Title ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Avatar", (object?)author.Avatar ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", author.Email);
                    cmd.Parameters.AddWithValue("@Id", author.Id);

                    if (cmd.ExecuteNonQuery() == 1)
                        return true;
                    return false;
                }
            }
        }

        public void DeleteAuthor(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string commandString = "DELETE FROM Authors WHERE Id = @Id";

                using (var cmd = new SqlCommand(commandString, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
