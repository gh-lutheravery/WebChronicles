using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebChronicles.Controllers.Data;
using WebChronicles.Models;

namespace WebChronicles.Controllers.Business
{
    public class ChapterBusiness
    {
        private readonly ChapterData _chapterData;
        private readonly StoryData _storyData;

        public ChapterBusiness(ChapterData chapterData, StoryData storyData)
        {
            _chapterData = chapterData;
            _storyData = storyData;
        }

        public List<Chapter> GetAllChapters(int storyId)
        {
            var chapters = _chapterData.GetAllChaptersByStoryId(storyId);
            chapters.OrderBy((x) => x.Posted);
            return chapters;
        }

        public Chapter? GetChapter(int id) 
        {
            Chapter? chapter = _chapterData.GetChapterById(id);
            return chapter;
        }
        
        public int? CreateChapter(Chapter chapter, ClaimsPrincipal claims)
        {
            // Verify user is allowed to create a chapter for this story
            string? idValue = claims.FindFirstValue("ID");
            Story? story = _storyData.GetStoryById(chapter.StoryId);
            if (story == null)
                return null;

            if (string.IsNullOrEmpty(idValue) || story.AuthorId != Int32.Parse(idValue))
                return null;

            chapter.Posted = DateTime.Now;

            int id = _chapterData.CreateChapter(chapter);
            return id;
        }
    }
}