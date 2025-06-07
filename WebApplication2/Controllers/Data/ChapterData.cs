using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebChronicles.Models;

namespace WebChronicles.Controllers.Data
{
    public class ChapterData
    {
        private readonly string _connectionString;

        public ChapterData(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public int CreateChapter(Chapter chapter)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string commandString = @"
                    INSERT INTO Chapters (Title, Posted, Content, StoryId)
                    OUTPUT INSERTED.Id
                    VALUES (@Title, @Posted, @Content, @StoryId)";

                using (var cmd = new SqlCommand(commandString, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", chapter.Title);
                    cmd.Parameters.AddWithValue("@Posted", chapter.Posted);
                    cmd.Parameters.AddWithValue("@Content", chapter.Content);
                    cmd.Parameters.AddWithValue("@StoryId", chapter.StoryId);

                    var chapterId = (int)cmd.ExecuteScalar();
                    return chapterId;
                }
            }
        }

        public Chapter? GetChapterById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string commandString = @"
                    SELECT Id, Title, Posted, Content, StoryId
                    FROM Chapters
                    WHERE Id = @Id";

                using (var cmd = new SqlCommand(commandString, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        int idOrdinal = reader.GetOrdinal("Id");
                        int titleOrdinal = reader.GetOrdinal("Title");
                        int postedOrdinal = reader.GetOrdinal("Posted");
                        int contentOrdinal = reader.GetOrdinal("Content");
                        int storyIdOrdinal = reader.GetOrdinal("StoryId");

                        if (reader.Read())
                        {
                            var chapter = new Chapter
                            {
                                Id = reader.GetInt32(idOrdinal),
                                Title = reader.GetString(titleOrdinal),
                                Posted = reader.GetDateTime(postedOrdinal),
                                Content = reader.GetString(contentOrdinal),
                                StoryId = reader.GetInt32(storyIdOrdinal)
                            };
                            return chapter;
                        }
                    }
                    return null;
                }
            }
        }

        public List<Chapter> GetAllChaptersByStoryId(int storyId)
        {
            var chapters = new List<Chapter>();
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string commandString = @"
                    SELECT Id, Title, Posted, Content, StoryId
                    FROM Chapters
                    WHERE StoryId = @StoryId";

                using (var cmd = new SqlCommand(commandString, conn))
                {
                    cmd.Parameters.AddWithValue("@StoryId", storyId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        int idOrdinal = reader.GetOrdinal("Id");
                        int titleOrdinal = reader.GetOrdinal("Title");
                        int postedOrdinal = reader.GetOrdinal("Posted");
                        int contentOrdinal = reader.GetOrdinal("Content");
                        int storyIdOrdinal = reader.GetOrdinal("StoryId");


                        while (reader.Read())
                        {
                            var chapter = new Chapter
                            {
                                Id = reader.GetInt32(idOrdinal),
                                Title = reader.GetString(titleOrdinal),
                                Posted = reader.GetDateTime(postedOrdinal),
                                Content = reader.GetString(contentOrdinal),
                                StoryId = reader.GetInt32(storyIdOrdinal)
                            };
                            chapters.Add(chapter);
                        }
                    }
                }
            }
            return chapters;
        }

        public bool UpdateChapter(Chapter chapter)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string commandString = @"
                    UPDATE Chapters
                    SET Title = @Title,
                        Posted = @Posted,
                        Content = @Content,
                        StoryId = @StoryId
                    WHERE Id = @Id";

                using (var cmd = new SqlCommand(commandString, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", chapter.Id);
                    cmd.Parameters.AddWithValue("@Title", (object?)chapter.Title ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Posted", chapter.Posted);
                    cmd.Parameters.AddWithValue("@Content", (object?)chapter.Content ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@StoryId", chapter.StoryId);

                    var result = cmd.ExecuteNonQuery() == 1;
                    return result;
                }
            }
        }

        public bool DeleteChapter(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                string commandString = "DELETE FROM Chapters WHERE Id = @Id";
                using (var cmd = new SqlCommand(commandString, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    var result = cmd.ExecuteNonQuery() == 1;
                    return result;
                }
            
            }
        }
    }
}
