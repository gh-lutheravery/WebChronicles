using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using WebApplication2.Models;
using Microsoft.Extensions.Configuration;

namespace WebApplication2.Controllers.Data
{
    public class StoryData
    {
        private readonly string _connectionString;
        private readonly AuthorData _authorData;

        public StoryData(IConfiguration configuration, AuthorData authorData)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _authorData = authorData;
        }

        public int CreateStory(Story story)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(@"
                INSERT INTO Stories (
                    Type, Title, Image, Status, AuthorId, Description, 
                    Posted, Followers, Favorites, Views
                ) 
                OUTPUT INSERTED.Id 
                VALUES (
                    @Type, @Title, @Image, @Status, @AuthorId, @Description, 
                    @Posted, @Followers, @Favorites, @Views
                )", conn);

            cmd.Parameters.AddWithValue("@Type", story.Type ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Title", story.Title ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Image", story.Image ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Status", story.Status ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@AuthorId", story.AuthorId);
            cmd.Parameters.AddWithValue("@Description", story.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@Posted", story.Posted);
            cmd.Parameters.AddWithValue("@Followers", story.Followers);
            cmd.Parameters.AddWithValue("@Favorites", story.Favorites);
            cmd.Parameters.AddWithValue("@Views", story.Views);

            conn.Open();
            var storyId = (int)cmd.ExecuteScalar();

            return storyId;
        }

        // Read (Get by Id)
        public Story? GetStoryById(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(@"
                SELECT Id, Type, Title, Image, Status, AuthorId, Description, 
                       Posted, Followers, Favorites, Views 
                FROM Stories 
                WHERE Id = @Id", conn);

            cmd.Parameters.AddWithValue("@Id", id);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                var story = new Story
                {
                    Id = reader.GetInt32(0),
                    Type = reader.IsDBNull(1) ? null : reader.GetString(1),
                    Title = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Image = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Status = reader.IsDBNull(4) ? null : reader.GetString(4),
                    AuthorId = reader.GetInt32(5),
                    Description = reader.IsDBNull(6) ? null : reader.GetString(6),
                    Posted = reader.GetDateTime(7),
                    Followers = reader.GetInt32(8),
                    Favorites = reader.GetInt32(9),
                    Views = reader.GetInt32(10)
                };

                story.Author = _authorData.GetAuthorById(story.AuthorId);

                story.Tags = GetStoryTags(id);

                return story;
            }
            return null;
        }

        public List<Story> GetAllStories()
        {
            var stories = new List<Story>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(@"
                SELECT Id, Type, Title, Image, Status, AuthorId, Description, 
                       Posted, Followers, Favorites, Views 
                FROM Stories", conn);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var story = new Story
                {
                    Id = reader.GetInt32(0),
                    Type = reader.IsDBNull(1) ? null : reader.GetString(1),
                    Title = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Image = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Status = reader.IsDBNull(4) ? null : reader.GetString(4),
                    AuthorId = reader.GetInt32(5),
                    Description = reader.IsDBNull(6) ? null : reader.GetString(6),
                    Posted = reader.GetDateTime(7),
                    Followers = reader.GetInt32(8),
                    Favorites = reader.GetInt32(9),
                    Views = reader.GetInt32(10)
                };

                story.Author = _authorData.GetAuthorById(story.AuthorId);

                story.Tags = GetStoryTags(story.Id);
                stories.Add(story);
            }
            return stories;
        }

        private ICollection<Tag> GetStoryTags(int storyId)
        {
            var tags = new List<Tag>();
            // TODO
            return tags;
        }
    }
}
    