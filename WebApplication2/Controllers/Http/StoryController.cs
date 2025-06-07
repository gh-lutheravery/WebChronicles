using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebChronicles.Controllers.Business;
using WebChronicles.Models;
using WebChronicles.ViewModels;

namespace WebChronicles.Controllers.Http
{
    public class StoryController : Controller
    {
        private readonly StoryBusiness _storyBusiness;
        private readonly ChapterBusiness _chapterBusiness;
        private readonly AuthorBusiness _authorBusiness;

        public StoryController(StoryBusiness storyBusiness, ChapterBusiness chapterBusiness, AuthorBusiness authorBusiness)
        {
            _storyBusiness = storyBusiness;
            _chapterBusiness = chapterBusiness;
            _authorBusiness = authorBusiness;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            if (id == 0) 
                return BadRequest();
            
            Story? story = _storyBusiness.GetStory(id);

            if (story == null) 
                return NotFound();

            story.Chapters = _chapterBusiness.GetAllChapters(id);
            story.Author = _authorBusiness.GetAuthor(story.AuthorId);

            return View(story);
        }

        public ActionResult ChapterDetails(int id)
        {
            if (id == 0)
                return BadRequest();

            ChapterViewModel? viewModel = _storyBusiness.GetChapterViewModel(id);
            if (viewModel == null)
                return BadRequest();

            return View(viewModel);
        }

        public ActionResult ChapterCreate(int storyId)
        {
            if (storyId == 0)
                return BadRequest();

            Story? story = _storyBusiness.GetStory(storyId);

            if (story == null)
                return NotFound();

            var chapter = new Chapter
            {
                Title = string.Empty,
                Content = string.Empty,
                Story = story,
            };

            return View(chapter);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChapterCreate(Chapter chapter)
        {
            try
            {
                int? assignedId = _chapterBusiness.CreateChapter(chapter, User);
                if (assignedId == null)
                    return BadRequest();

                return RedirectToAction("ChapterDetails", new { id = assignedId });
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Story userStory)
        {
            try
            {
                int? assignedId = _storyBusiness.CreateStory(userStory, User);
                if (assignedId == null)
                    return BadRequest();
                
                return RedirectToAction("Details", new { id = assignedId });
            }
            catch
            {
                return View();
            }
        }

        

        public ActionResult Edit(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
