using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebChronicles.Controllers.Data;
using WebChronicles.ViewModels;
using WebChronicles.Controllers.Business;

namespace WebChronicles.Controllers.Http
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly StoryBusiness _storyBusiness;

        public HomeController(ILogger<HomeController> logger, StoryBusiness storyBusiness)
        {
            _logger = logger;
            _storyBusiness = storyBusiness;
        }

        public IActionResult Index()
        {
            var stories = _storyBusiness.GetAllStories();
            return View(stories);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
