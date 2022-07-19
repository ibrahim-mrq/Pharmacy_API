using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pharmacy.Controllers
{
    [Route("api/Author")]
    [ApiController]
    public class AuthorController : ControllerBase
    {

        public static List<Author> Authors = new List<Author>() {
                new Author(){ Id = 1,Name  = "Ibrahim" ,Location=   "Gaza"},
                new Author(){Id = 2,Name  = "Ahmed" ,Location=   "Gaza"},
                new Author(){Id = 3,Name  = "Ali" ,Location=   "Gaza"},
            };


        [HttpGet]
        public List<Author> getAllAuthors()
        {
            return Authors;
        }


        [HttpGet("{Id}")]
        public IActionResult GetAuthor(int Id)
        {
            var currentUser = Authors.Where(x => x.Id == Id).FirstOrDefault();
            if (currentUser == null){
                return NotFound(new { code = 320 , message = "Invalid Id"});
            }
            else {
                return Ok(currentUser);
            }
        }

        [HttpPost]
        public IActionResult AddAuthor(Author author)
        {
            if (String.IsNullOrWhiteSpace(author.Name))
            {

            }
            Authors.Add(author);
            return Ok();
        }


    }
}
