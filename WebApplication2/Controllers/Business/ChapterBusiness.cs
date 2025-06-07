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
        private readonly StoryBusiness _storyBusiness;

        public ChapterBusiness(ChapterData chapterData, StoryBusiness storyBusiness)
        {
            _chapterData = chapterData;
            _storyBusiness = storyBusiness;
        }

        public List<Chapter> GetAllChapters(int storyId)
        {
            return _chapterData.GetAllChaptersByStoryId(storyId);
        }

        public Chapter? GetChapter(int id) 
        {
            Chapter? chapter = _chapterData.GetChapterById(id);
            return chapter;
        }
        
        public int? CreateChapter(Chapter chapter, ClaimsPrincipal claims)
        {
            // Verify user is authenticated
            string? idValue = claims.FindFirstValue("ID");
            Story? story = _storyBusiness.GetStory(chapter.StoryId);
            if (story == null)
                return null;

            if (string.IsNullOrEmpty(idValue) || story.AuthorId == Int32.Parse(idValue))
                return null;

            chapter.Posted = DateTime.Now;

            int id = _chapterData.CreateChapter(chapter);
            return id;
        }
    }
}