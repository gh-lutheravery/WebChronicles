using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using WebApplication2.Controllers.Business;
using WebApplication2.Models;
using WebApplication5.ViewModels.User;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebApplication2.Controllers.Http
{
    public class AuthorController : Controller
    {
        private readonly AuthorBusiness _authorBusiness;

        public AuthorController(AuthorBusiness authorBusiness)
        {
            _authorBusiness = authorBusiness;
        }

        // GET: AuthorController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AuthorController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AuthorController/Create
        public ActionResult Create()
        {
            var vm = new RegisterViewModel();
            return View("Register", vm);
        }

        // POST: AuthorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RegisterViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _authorBusiness.Register(vm, new PasswordHasher<Author>());
                return RedirectToAction(nameof(HomeController.Index));
            }
            else
            {
                return View(vm);
            }
        }

        // GET: AuthorController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AuthorController/Edit/5
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

        // GET: AuthorController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AuthorController/Delete/5
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
