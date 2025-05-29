using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using static System.Runtime.InteropServices.JavaScript.JSType;
using WebChronicles.Models;
using WebChronicles.Controllers.Data;
using WebChronicles.ViewModels;

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
