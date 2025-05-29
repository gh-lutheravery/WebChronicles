using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebChronicles.Models;

namespace WebChronicles.Controllers.Data
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
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string commandString = @"
                    INSERT INTO Stories (Type, Title, Image, Status, AuthorId, Description, Posted, Followers, Favorites, Views) 
                    OUTPUT INSERTED.Id 
                    VALUES (@Type, @Title, @Image, @Status, @AuthorId, @Description, @Posted, @Followers, @Favorites, @Views)";

                using (var cmd = new SqlCommand(commandString, conn))
                {
                    cmd.Parameters.AddWithValue("@Type", (object?)story.Type ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Title", (object?)story.Title ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Image", (object?)story.Image ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", (object?)story.Status ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@AuthorId", story.AuthorId);
                    cmd.Parameters.AddWithValue("@Description", (object?)story.Description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Posted", story.Posted);
                    cmd.Parameters.AddWithValue("@Followers", story.Followers);
                    cmd.Parameters.AddWithValue("@Favorites", story.Favorites);
                    cmd.Parameters.AddWithValue("@Views", story.Views);

                    var storyId = (int)cmd.ExecuteScalar();
                    return storyId;
                }
            }
        }

        public Story? GetStoryById(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string commandString = @"
                SELECT Id, Type, Title, Image, Status, AuthorId, Description, Posted, Followers, Favorites, Views FROM Stories 
                WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(commandString, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        int idOrdinal = reader.GetOrdinal("Id");
                        int typeOrdinal = reader.GetOrdinal("Type");
                        int titleOrdinal = reader.GetOrdinal("Title");
                        int imageOrdinal = reader.GetOrdinal("Image");
                        int statusOrdinal = reader.GetOrdinal("Status");
                        int authorIdOrdinal = reader.GetOrdinal("AuthorId");
                        int descriptionOrdinal = reader.GetOrdinal("Description");
                        int postedOrdinal = reader.GetOrdinal("Posted");
                        int followersOrdinal = reader.GetOrdinal("Followers");
                        int favoritesOrdinal = reader.GetOrdinal("Favorites");
                        int viewsOrdinal = reader.GetOrdinal("Views");

                        if (reader.Read())
                        {
                            var story = new Story
                            {
                                Id = reader.GetInt32(idOrdinal),
                                Type = reader.GetString(typeOrdinal),
                                Title = reader.GetString(titleOrdinal),
                                Image = reader.IsDBNull(imageOrdinal) ? null : reader.GetString(imageOrdinal),
                                Status = reader.GetString(statusOrdinal),
                                AuthorId = reader.GetInt32(authorIdOrdinal),
                                Description = reader.IsDBNull(descriptionOrdinal) ? null : reader.GetString(descriptionOrdinal),
                                Posted = reader.GetDateTime(postedOrdinal),
                                Followers = reader.GetInt32(followersOrdinal),
                                Favorites = reader.GetInt32(favoritesOrdinal),
                                Views = reader.GetInt32(viewsOrdinal)
                            };

                            return story;
                        }
                    }
                    
                    return null;
                }
            }
            
        }

        public List<Story> GetAllStories()
        {
            var stories = new List<Story>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string commandString = @"
                    SELECT Id, Type, Title, Image, Status, AuthorId, Description, Posted, Followers, Favorites, Views 
                    FROM Stories";

                using (var cmd = new SqlCommand(commandString, conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        int idOrdinal = reader.GetOrdinal("Id");
                        int typeOrdinal = reader.GetOrdinal("Type");
                        int titleOrdinal = reader.GetOrdinal("Title");
                        int imageOrdinal = reader.GetOrdinal("Image");
                        int statusOrdinal = reader.GetOrdinal("Status");
                        int authorIdOrdinal = reader.GetOrdinal("AuthorId");
                        int descriptionOrdinal = reader.GetOrdinal("Description");
                        int postedOrdinal = reader.GetOrdinal("Posted");
                        int followersOrdinal = reader.GetOrdinal("Followers");
                        int favoritesOrdinal = reader.GetOrdinal("Favorites");
                        int viewsOrdinal = reader.GetOrdinal("Views");

                        while (reader.Read())
                        {
                            var story = new Story
                            {
                                Id = reader.GetInt32(idOrdinal),
                                Type = reader.GetString(typeOrdinal),
                                Title = reader.GetString(titleOrdinal),
                                Image = reader.IsDBNull(imageOrdinal) ? null : reader.GetString(imageOrdinal),
                                Status = reader.GetString(statusOrdinal),
                                AuthorId = reader.GetInt32(authorIdOrdinal),
                                Description = reader.IsDBNull(descriptionOrdinal) ? null : reader.GetString(descriptionOrdinal),
                                Posted = reader.GetDateTime(postedOrdinal),
                                Followers = reader.GetInt32(followersOrdinal),
                                Favorites = reader.GetInt32(favoritesOrdinal),
                                Views = reader.GetInt32(viewsOrdinal)
                            };

                            stories.Add(story);
                        }
                    }
                }
            }
            return stories;
        }

        // Update
        public bool UpdateStory(Story story)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string commandString = @"
                    UPDATE Stories
                    SET Type = @Type,
                        Title = @Title,
                        Image = @Image,
                        Status = @Status,
                        AuthorId = @AuthorId,
                        Description = @Description,
                        Posted = @Posted,
                        Followers = @Followers,
                        Favorites = @Favorites,
                        Views = @Views
                    WHERE Id = @Id";

                using (var cmd = new SqlCommand(commandString, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", story.Id);
                    cmd.Parameters.AddWithValue("@Type", (object?)story.Type ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Title", (object?)story.Title ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Image", (object?)story.Image ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Status", (object?)story.Status ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@AuthorId", story.AuthorId);
                    cmd.Parameters.AddWithValue("@Description", (object?)story.Description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Posted", story.Posted);
                    cmd.Parameters.AddWithValue("@Followers", story.Followers);
                    cmd.Parameters.AddWithValue("@Favorites", story.Favorites);
                    cmd.Parameters.AddWithValue("@Views", story.Views);

                    var result = cmd.ExecuteNonQuery() > 0;
                    // Update Tags relationship if needed
                    // if (story.Tags != null) { UpdateStoryTags(story.Id, story.Tags); }
                    return result;
                }
            }
        }

        // Delete
        public bool DeleteStory(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // First delete related records in junction tables if needed
                // DeleteStoryTags(id);
                // DeleteStoryChapters(id);

                string commandString = "DELETE FROM Stories WHERE Id = @Id";
                using (var cmd = new SqlCommand(commandString, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }

        // Helper methods for handling relationships
        public List<Tag> GetStoryTags(int storyId)
        {
            var tags = new List<Tag>();
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(@"
                SELECT t.Id, t.Title
                FROM StoryTags st 
                JOIN Tags t ON st.TagId = t.Id 
                WHERE st.StoryId = @StoryId", conn);
            cmd.Parameters.AddWithValue("@StoryId", storyId);
            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                tags.Add(new Tag
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1)
                });
            }
            return tags;
        }
    }
}