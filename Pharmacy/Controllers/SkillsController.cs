using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pharmacy.Controllers
{
    // [Route("api/user/{userId}/skills")]
    [Route("api")]
    [ApiController]
    public class SkillsController : ControllerBase
    {

        public static List<Skills> skills = new List<Skills>() {
                new Skills(){ Id = 1,Name  = "Ibrahim" ,UserId = 1 },
                new Skills(){ Id = 2,Name  = "Ibrahim" ,UserId = 2 },
                new Skills(){ Id = 3,Name  = "Ibrahim" ,UserId = 3 },
                new Skills(){ Id = 4,Name  = "Ibrahim" ,UserId = 1 }
        };

        [HttpGet, Route("skills")]
        public IActionResult getSkills()
        {
            return Ok(new { success = true, message = "success", code = UserController.success, skills = skills });
        }

        [HttpGet, Route("getUserSkills")]
        public IActionResult getUserSkills([FromQuery()] int userId)
        {
            var userSkills = skills.Where(x => x.UserId == userId).ToList();
            return Ok(new { success = true, message = "success", code = UserController.success, skills = userSkills });
        }

        [HttpGet(), Route("getUserSkillsById")]
        public IActionResult getUserSkillsById([FromQuery()] int userId, [FromQuery()] int skillsId)
        {
            var currentSkills = skills.Where(x => x.UserId == userId && x.Id == skillsId).FirstOrDefault();
            if (currentSkills == null)
            {
                return NotFound(new { success = false, message = "Skills Not Found", code = UserController.notFound });
            }
            return Ok(new { success = true, message = "success", code = UserController.success, skills = currentSkills });
        }

        [HttpGet(), Route("getSkillsById/{skillsId}")]
        public IActionResult getSkillsById([FromRoute] int skillsId)
        {
            var currentSkills = skills.Where(x => x.Id == skillsId).FirstOrDefault();
            if (currentSkills == null)
            {
                return NotFound(new { success = false, message = "Skills Not Found", code = UserController.notFound });
            }
            return Ok(new { success = true, message = "success", code = UserController.success, skills = currentSkills });
        }

        [HttpPost(), Route("addSkills/{Id}")]
        public IActionResult addSkills([FromRoute] int Id, [FromForm] Skills newSkills, [FromHeader] String token)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return Unauthorized(new { success = false, message = "Invalid token", code = UserController.unauthorized });
            }
           /* if (!UserController.users.Any(x => x.Id == Id))
            {
                return NotFound(new { success = false, message = "User Not Exists ", code = UserController.notFound });
            }*/
            if (Id != newSkills.UserId)
            {
                return BadRequest(new { success = false, message = "Invalid User Id ", code = UserController.invalidValue });
            }
            if (skills.Any(x => x.UserId == Id && x.Id == newSkills.Id))
            {
                return Conflict(new { success = false, message = "Skills is Already Exists ", code = UserController.conflict });
            }
            skills.Add(newSkills);

            /*var currentUser = UserController.users.Where(x => x.Id == Id).SingleOrDefault();
            if (currentUser == null)
            {
                return NotFound(new { success = false, message = "User Not Found", code = UserController.notFound });
            }
            currentUser.Skills.Add(newSkills);
            */
            return CreatedAtAction(nameof(getSkillsById), new { UserId = Id, skillsId = newSkills.Id },
                new { success = true, message = "Created", code = UserController.created, skills = newSkills });
        }


    }
}
