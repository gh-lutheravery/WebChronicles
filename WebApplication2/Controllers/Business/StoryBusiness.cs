using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.Controllers.Data;

namespace WebApplication2.Controllers.Business
{
    public class StoryBusiness
    {
        private readonly StoryData _storyData;

        public StoryBusiness(StoryData storyData)
        {
            _storyData = storyData;
        }

        public List<Story> GetAllStories()
        {
            return _storyData.GetAllStories();
        }
    }
}
