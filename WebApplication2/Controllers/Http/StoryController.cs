using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebChronicles.Controllers.Business;
using WebChronicles.Models;

namespace WebChronicles.Controllers.Http
{
    public class StoryController : Controller
    {
        private readonly StoryBusiness _storyBusiness;
        public StoryController(StoryBusiness storyBusiness) 
        {
            _storyBusiness = storyBusiness;    
        }

        // GET: StoryController
        public ActionResult Index()
        {
            return View();
        }

        // GET: StoryController/Details/5
        public ActionResult Details(int id)
        {
            if (id == 0) 
            { 
                return BadRequest();
            }
            Story? story = _storyBusiness.GetStory(id);

            if (story == null) {
                return NotFound();
            }
            return View(story);
        }

        // GET: StoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: StoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Story userStory)
        {
            try
            {
                int assignedId = _storyBusiness.CreateStory(userStory);
                return RedirectToAction("Index", "Home");
            }
            catch
            {
                return View();
            }
        }

        // GET: StoryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: StoryController/Edit/5
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

        // GET: StoryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: StoryController/Delete/5
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
