using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebChronicles.Controllers.Data;
using WebChronicles.Models;
using WebChronicles.ViewModels;

namespace WebChronicles.Controllers.Business
{
    public class StoryBusiness
    {
        private readonly StoryData _storyData;
        private readonly AuthorData _authorData;

        private readonly ChapterBusiness _chapterBusiness;

        public StoryBusiness(StoryData storyData, AuthorData authorData, ChapterBusiness chapterBusiness)
        {
            _storyData = storyData;
            _authorData = authorData;
            _chapterBusiness = chapterBusiness;
        }

        public List<Story> GetAllStories()
        {
            return _storyData.GetAllStories();
        }

        public Story? GetStory(int id) 
        {
            Story? story = _storyData.GetStoryById(id);
            if (story != null) 
            {
                story.Author = _authorData.GetAuthorById(story.AuthorId);
                //story.Tags = _storyData.GetStoryTags(id);
            }
            return story;
        }
        
        public int? CreateStory(Story userStory, ClaimsPrincipal claims)
        {
            string? idValue = claims.FindFirstValue("ID");
            if (string.IsNullOrEmpty(idValue))
                return null;
            
            int userId = Int32.Parse(idValue);
            userStory.Views = 0;
            userStory.Favorites = 0;
            userStory.Followers = 0;
            userStory.Posted = DateTime.Now;
            userStory.AuthorId = userId;

            int id = _storyData.CreateStory(userStory);
            return id;
        }

        internal ChapterViewModel? GetChapterViewModel(int id)
        {
            ChapterViewModel viewModel = new ChapterViewModel();

            var currentChapter = _chapterBusiness.GetChapter(id);
            if (currentChapter == null)
                return null;

            int storyId = currentChapter.StoryId;

            Story? story = GetStory(storyId);
            if (story == null)
                return null;

            var chapters = _chapterBusiness.GetAllChapters(storyId);

            // find current chapter's index
            var currentIndex = chapters.FindIndex(c => c.Id == id);
            if (currentIndex != -1)
            {
                viewModel.PreviousChapterId = currentIndex > 0 ? chapters[currentIndex - 1].Id : 0;
                viewModel.NextChapterId = currentIndex < chapters.Count - 1 ? chapters[currentIndex + 1].Id : 0;
            }
            else
            {
                viewModel.PreviousChapterId = 0;
                viewModel.NextChapterId = 0;
            }

            viewModel.Story = story;
            viewModel.Chapter = currentChapter;

            return viewModel;
        }
    }
}
