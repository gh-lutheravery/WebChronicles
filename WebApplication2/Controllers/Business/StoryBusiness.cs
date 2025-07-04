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

        public StoryBusiness(StoryData storyData, AuthorData authorData)
        {
            _storyData = storyData;
            _authorData = authorData;
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
    }
}
