using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using WebChronicles.Controllers.Business;
using WebChronicles.Models;
using WebChronicles.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebChronicles.Controllers.Http
{
    public class AuthorController : Controller
    {
        private readonly AuthorBusiness _authorBusiness;

        public AuthorController(AuthorBusiness authorBusiness)
        {
            _authorBusiness = authorBusiness;
        }

        public ActionResult List()
        {
            List<Author> authors = _authorBusiness.GetAllAuthors();
            return View(authors);
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AuthorController/Create
        public ActionResult Register()
        {
            var vm = new RegisterViewModel();
            return View("Register", vm);
        }

        // POST: AuthorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _authorBusiness.Register(vm, new PasswordHasher<Author>());
                return RedirectToAction("Index", "Home");
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
