using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication2.Models;
using WebApplication2.Controllers.Data;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;
using WebApplication5.ViewModels.User;

namespace WebApplication2.Controllers.Business
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

        public void Register(RegisterViewModel vm, PasswordHasher<Author> hasher)
        {
            string hashed = hasher.HashPassword(new Author(), vm.Password);

            Author profile = new Author();
            profile.Name = vm.Name;
            profile.Email = vm.Email;
            profile.Password = hashed;
            profile.Joined = DateTime.UtcNow;

            _authorData.CreateAuthor(profile);
        }
    }
}
