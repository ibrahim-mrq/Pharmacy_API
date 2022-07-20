using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Models;
using Pharmacy.Models.Helper;
using Pharmacy.Models.RequestDTO;
using Pharmacy.Models.ResponseDTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pharmacy.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserController : ControllerBase
    {

        public static int success = 200;
        public static int created = 201;
        public static int invalidValue = 400;
        public static int unauthorized = 401;
        public static int notFound = 404;
        public static int conflict = 502;

        public static List<User> users = new List<User>()
        {
            new User(){Id = 1,Name = "Ibrahim" ,Location = "Gaza", IsDeleted = false  },
            new User(){Id = 2,Name = "Ahmed" ,Location = "KhanYunis", IsDeleted = false },
            new User(){Id = 3,Name = "Ali" ,Location = "Rafah", IsDeleted = true },
        };

        public readonly IMapper Mapper;

        public UserController(IMapper mapper)
        {
            Mapper = mapper;
        }

        [HttpGet(), Route("getAllUsers")]
        public IActionResult getAllUsers()
        {
            return Ok(new
            {
                success = true,
                message = "success",
                code = success,
                users = users
                //.Where(x => x.IsDeleted = false)
                // .Select(x => UserHelper.MapDomainToResponse(x))
                .Select(x => Mapper.Map<UserResponseDTO>(x))
            });
        }

        [HttpGet(), Route("getUserById/{Id}")]
        public IActionResult GetUserById([FromRoute] int Id, [FromHeader] String token)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return Unauthorized(new { success = false, message = "Invalid token", code = unauthorized });
            }
            var currentUser = users.Where(x => x.Id == Id && x.IsDeleted == false).SingleOrDefault();
            if (currentUser == null)
            {
                return BadRequest(new { success = false, message = "user not found", code = invalidValue });
            }
            return Ok(new { success = true, message = "success", code = success, user = Mapper.Map<UserResponseDTO>(currentUser) });

        }

        [HttpGet(), Route("searchUser")]
        public IActionResult SearchUser([FromQuery] String Name)
        {
            var query = users.AsQueryable();
            query = query.Where(x => x.IsDeleted == false);
            if (!String.IsNullOrWhiteSpace(Name))
            {
                query = query.Where(x => x.Name.Contains(Name, StringComparison.OrdinalIgnoreCase));
            }
            query = query.OrderBy(x => x.Name) // تصاعدي
                .ThenBy(x => x.Location);
            ;
            //  query = query.OrderByDescending(x => x.Name); // تنازلي
            return Ok(new { success = true, message = "success", code = success, user = query.Select(x => Mapper.Map<UserResponseDTO>(x)) });
        }

        [HttpPost(), Route("createUser")]
        public IActionResult AddUser([FromForm] UserAddRequestDTO user, [FromHeader] String token)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return Unauthorized(new { success = false, message = "Invalid token", code = unauthorized });
            }
            /*if (String.IsNullOrWhiteSpace(user.Name))
            {
                return BadRequest(new { success = false, message = "Invalid Empty Name", code = invalidValue });
            }*/
            var currentUser = Mapper.Map<User>(user);
            currentUser.Id = users.Max(x => x.Id) + 1;
            users.Add(currentUser);
            return CreatedAtAction(nameof(GetUserById), new { Id = currentUser.Id },
                new { success = true, message = "Created", code = created, user = UserHelper.MapDomainToResponse(currentUser) });
        }

        [HttpPut(), Route("updateUser/{Id}")]
        public IActionResult UpdateUser([FromRoute] int Id, [FromForm] UserUpdateRequestDTO user, [FromHeader] String token)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return Unauthorized(new { success = false, message = "Invalid token", code = unauthorized });
            }
            if (String.IsNullOrWhiteSpace(user.Name))
            {
                return BadRequest(new { success = false, message = "Invalid Empty Name", code = invalidValue });
            }
            var currentUser = users.Where(x => x.Id == Id && x.IsDeleted == false).SingleOrDefault();
            if (currentUser == null)
            {
                return NotFound(new { success = false, message = "User Not Found", code = notFound });
            }
            Mapper.Map(user, currentUser);
            return Ok(new { success = true, message = "update successfull", code = success, user = currentUser });
        }

        [HttpPatch("{id}")]
        public IActionResult updateUserPartially(int id, [FromHeader] String token, JsonPatchDocument authorPatch)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return Unauthorized(new { success = false, message = "Invalid token", code = unauthorized });
            }
            var currentUser = users.Where(x => x.Id == id && x.IsDeleted == false).SingleOrDefault();
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
            var currentUser = users.Where(x => x.Id == id && x.IsDeleted == false).SingleOrDefault();
            if (currentUser == null)
            {
                return NotFound(new { success = false, message = "User Not Found", code = notFound });
            }
            var currentSkills = SkillsController.skills.Where(x => x == currentUser.Skills.FirstOrDefault()).FirstOrDefault();
            if (currentSkills != null)
            {
                SkillsController.skills.Remove(currentSkills);
            }
            //   users.Remove(currentUser);
            currentUser.IsDeleted = true;
            return Ok(new { success = true, message = "delete successfull", code = success, user = currentUser });
        }

    }
}
