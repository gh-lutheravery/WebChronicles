using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;
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
            if (id == 0)
                return BadRequest();

            Author? author = _authorBusiness.GetAuthor(id);
            if (author == null)
                return NotFound();

            return View(author);
        }

        public ActionResult Login()
        {
            var vm = new LoginViewModel();
            return View("Login", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var loginInfo = _authorBusiness.AuthenticateUser(vm, new PasswordHasher<Author>());
                if (loginInfo == null)
                {
                    ViewData["IncorrectInput"] = "Your email or password is incorrect; try again.";
                    return View(vm);
                }

                var claims = loginInfo.Value.Item1;
                var authProperties = loginInfo.Value.Item2;

                // sends the cookie to the browser
                HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claims),
                    authProperties);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(vm);
            }
        }

        public ActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            var vm = new RegisterViewModel();
            return View("Register", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _authorBusiness.Register(vm, new PasswordHasher<Author>());
                return RedirectToAction("Login");
            }
            else
            {
                return View(vm);
            }
        }

        public IActionResult Update(int id)
        {
            // checks if this user is the one they are trying to update
            if (!User.Claims.Any(c => c.Type == "ID" && c.Value == id.ToString()))
                return Unauthorized();
            
            // get existing data to populate form
            var author = _authorBusiness.GetAuthor(id);

            if (author == null)
                return NotFound();

            return View(author);
        }

        [HttpPost]
        public IActionResult Update(Author author)
        {
            if (!User.Claims.Any(c => c.Type == "ID" && c.Value == author.Id.ToString()))
                return Unauthorized();

            bool updateSuccess = _authorBusiness.UpdateAuthor(author);

            if (updateSuccess)
            {
                return RedirectToAction("Details", new { id = author.Id });
            }
            else
            {
                ModelState.AddModelError("", "Please try again.");
                return View(author);
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
