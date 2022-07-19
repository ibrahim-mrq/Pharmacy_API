using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pharmacy.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {

        public static int success = 200;
        public static int created = 201;
        public static int invalidValue = 400;
        public static int unauthorized = 401;
        public static int notFound = 404;
        public static int conflict = 502;

        public static List<User> users = new List<User>() {
                new User(){ Id = 1,Name  = "Ibrahim" ,Location=   "Gaza"},
                new User(){Id = 2,Name  = "Ahmed" ,Location=   "Gaza"},
                new User(){Id = 3,Name  = "Ali" ,Location=   "Gaza"},
        };


        [HttpGet]
        public IActionResult getAllUsers()
        {
            return Ok(new { success = true, message = "success", code = success, users = users });
        }

        [HttpGet("{Id}")]
        public IActionResult GetUserById(int Id)
        {
            var currentUser = users.Where(x => x.Id == Id).FirstOrDefault();
            if (currentUser == null)
            {
                return BadRequest(new { success = false, message = "user not found", code = invalidValue });
            }
            else
            {
                return Ok(new { success = true, message = "success", code = success, user = currentUser });
            }
        }

        [HttpPost]
        public IActionResult AddUser([FromForm] User user, [FromHeader] String token)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return Unauthorized(new { success = false, message = "Invalid token", code = unauthorized });
            }
            if (String.IsNullOrWhiteSpace(user.Name))
            {
                return BadRequest(new { success = false, message = "Invalid Empty Name", code = invalidValue });
            }
            var currentUser = users.Where(x => x.Id == user.Id).SingleOrDefault();
            if (currentUser != null)
            {
                return Conflict(new { success = false, message = "Duplicate Id", code = conflict });
            }
            users.Add(user);
            return CreatedAtAction(nameof(GetUserById), new { Id = user.Id }, new { success = true, message = "Created", code = created, user = user });
        }

        [HttpPut]
        public IActionResult updateUser([FromForm] User user, [FromHeader] String token)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return Unauthorized(new { success = false, message = "Invalid token", code = unauthorized });
            }
            if (String.IsNullOrWhiteSpace(user.Name))
            {
                return BadRequest(new { success = false, message = "Invalid Empty Name", code = invalidValue });
            }
            var currentUser = users.Where(x => x.Id == user.Id).SingleOrDefault();
            if (currentUser == null)
            {
                return NotFound(new { success = false, message = "User Not Found", code = notFound });
            }
            currentUser.Name = user.Name;
            currentUser.Location = user.Location;

            return Ok(new { success = true, message = "update successfull", code = success, user = currentUser });
        }

        [HttpPatch("{id}")]
        public IActionResult updateUserPartially(int id, [FromHeader] String token, /*[FromForm]*/ JsonPatchDocument authorPatch)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return Unauthorized(new { success = false, message = "Invalid token", code = unauthorized });
            }
            var currentUser = users.Where(x => x.Id == id).SingleOrDefault();
            if (currentUser == null)
            {
                return NotFound(new { success = false, message = "User Not Found", code = notFound });
            }
            authorPatch.ApplyTo(currentUser);
            return Ok(new { success = true, message = "update successfull", code = success, user = currentUser });
        }

        [HttpDelete("{id}")]
        public IActionResult deleteUser(int id, [FromHeader] String token)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return Unauthorized(new { success = false, message = "Invalid token", code = unauthorized });
            }
            var currentUser = users.Where(x => x.Id == id).SingleOrDefault();
            if (currentUser == null)
            {
                return NotFound(new { success = false, message = "User Not Found", code = notFound });
            }
            users.Remove(currentUser);
            return Ok(new { success = true, message = "delete successfull", code = success, user = currentUser });
        }

    }
}
