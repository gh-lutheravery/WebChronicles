using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;
using WebChronicles.Models;
using WebChronicles.Controllers.Data;
using WebChronicles.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace WebChronicles.Controllers.Business
{
    public class AuthorBusiness
    {
        private readonly AuthorData _authorData;

        public AuthorBusiness(AuthorData authorData)
        {
            _authorData = authorData;
        }

        public List<Author> GetAllAuthors()
        {
            return _authorData.GetAllAuthors();
        }

        public Author? GetAuthor(int id) 
        {
            Author? author = _authorData.GetAuthorById(id);
            return author;
        }

        public void Register(RegisterViewModel vm, PasswordHasher<Author> hasher)
        {
            Author profile = new Author() 
            {
                Name = vm.Name,
                Email = vm.Email,
                Password = string.Empty,
                Joined = DateTime.UtcNow
            };
            string hashed = hasher.HashPassword(profile, vm.Password);
            profile.Password = hashed;

            _authorData.CreateAuthor(profile);
        }

        public Author? ValidateLogin(LoginViewModel vm, PasswordHasher<Author> hasher)
        {
            Author? author = _authorData.GetAuthorByEmail(vm.Email);

            if (author == null)
                return null;

            var result = hasher.VerifyHashedPassword(author, author.Password, vm.Password);
            if (result == PasswordVerificationResult.Success)
                return author;
            else
                return null;
        }


        // if user is found, make a claim based on the user id and configure cookie settings
        public (ClaimsIdentity, AuthenticationProperties)?
            AuthenticateUser(LoginViewModel vm, PasswordHasher<Author> hasher)
        { 
            var profile = ValidateLogin(vm, hasher);
            if (profile == null)
                return null;

            var claims = new List<Claim>
            {
                new Claim("ID", profile.Id.ToString()),
            };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(1),
                IsPersistent = false,
                RedirectUri = ""
            };

            return (claimsIdentity, authProperties);
        }

        //UpdateAuthor
        public bool UpdateAuthor(Author author)
        {
            return _authorData.UpdateAuthor(author);
        }
    }
}
