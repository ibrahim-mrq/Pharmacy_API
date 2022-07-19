using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pharmacy.Controllers
{
    [Route("api/user/{userId}/skills")]
    [ApiController]
    public class SkillsController : ControllerBase
    {

        public static List<Skills> skills = new List<Skills>() {
                new Skills(){ Id = 1,Name  = "Ibrahim" ,UserId = 1 },
                new Skills(){ Id = 2,Name  = "Ibrahim" ,UserId = 2 },
                new Skills(){ Id = 3,Name  = "Ibrahim" ,UserId = 3 },
                new Skills(){ Id = 4,Name  = "Ibrahim" ,UserId = 1 }
        };
    
        [HttpGet]
        public IActionResult getAllUserSkills(int userId)
        {
            var userSkills = skills.Where(x => x.UserId == userId).ToList();
            return Ok(new { success = true, message = "success", code = UserController.success, skills = userSkills });
        }

        [HttpGet("{skillsId}")]
        public IActionResult getAllUserSkills(int UserId, int skillsId)
        {
            var currentSkills = skills.Where(x => x.UserId == UserId && x.Id == skillsId).FirstOrDefault();
            if (currentSkills == null)
            {
                return NotFound(new { success = false, message = "Skills Not Found", code = UserController.notFound });
            }
            return Ok(new { success = true, message = "success", code = UserController.success, skills = currentSkills });
        }

        [HttpGet("{skillsId}")]
        public IActionResult addNewSkills(int UserId, [FromForm] Skills skills, [FromHeader] String token)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return Unauthorized(new { success = false, message = "Invalid token", code = UserController.unauthorized });
            }
            if (String.IsNullOrWhiteSpace(skills.Name))
            {
                return BadRequest(new { success = false, message = "Invalid Empty Name", code = UserController. invalidValue });
            }
            var currentUser = UserController. users.Where(x => x.Id == UserId).SingleOrDefault();
            if (currentUser != null)
            {
                return Conflict(new { success = false, message = "Duplicate Id", code = conflict });
            }
            users.Add(user);
            return CreatedAtAction(nameof(GetUserById), new { Id = user.Id }, new { success = true, message = "Created", code = created, user = user });

        }
    }
}
