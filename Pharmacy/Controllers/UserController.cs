using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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


        public readonly IMapper Mapper;
        public readonly DBContext dbContext;
        private readonly IConfiguration configuration;

        public UserController(IMapper mapper, DBContext DBContext, IConfiguration configuration)
        {
            Mapper = mapper;
            this.dbContext = DBContext;
            this.configuration = configuration;
        }

        [HttpGet(nameof(getAllUsers), Name = "getAllUsers"), Route("getAllUsers")]
        public IActionResult getAllUsers([FromQuery] PagingDTO Page)
        {
            var ApiCompany = configuration.GetValue<String>("ApiSettings:ApiCompany");

            var list = dbContext.Users.AsQueryable()
                .Where(x => x.IsDeleted == false)
                .Select(x => Mapper.Map<UserResponseDTO>(x));

            var respone = new Response<UserResponseDTO>(list, Page)
            {
                success = true,
                message = "success",
                code = success
            };
            if (respone.Pageing.hasNextPage)
            {
                respone.Pageing.NextPageUrl = Url.Link("getAllUsers", new { Page.RowCount, PageNumber = Page.PageNumber + 1 });
            }
            if (respone.Pageing.hasPrevPage)
            {
                respone.Pageing.PrevPageUrl = Url.Link("getAllUsers", new { Page.RowCount, PageNumber = Page.PageNumber - 1 });
            }
            return Ok(respone);
        }

        [HttpGet(), Route("getUserById/{Id}")]
        public IActionResult GetUserById([FromRoute] int Id, [FromHeader] String token)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return Unauthorized(new { success = false, message = "Invalid token", code = unauthorized });
            }
            var currentUser = dbContext.Users.Where(x => x.Id == Id && x.IsDeleted == false).SingleOrDefault();
            if (currentUser == null)
            {
                return BadRequest(new { success = false, message = "user not found", code = invalidValue });
            }
            return Ok(new { success = true, message = "success", code = success, user = Mapper.Map<UserResponseDTO>(currentUser) });

        }

        [HttpGet(), Route("searchUser")]
        public IActionResult SearchUser([FromQuery] String Name)
        {
            var query = dbContext.Users.AsQueryable();
            query = query.Where(x => x.IsDeleted == false);
            if (!String.IsNullOrWhiteSpace(Name))
            {
                query = query.Where(x => x.Name.Contains(Name, StringComparison.OrdinalIgnoreCase));
            }
            query = query.OrderBy(x => x.Name)
                .ThenBy(x => x.Location);
            ;
            return Ok(new { success = true, message = "success", code = success, user = query.Select(x => Mapper.Map<UserResponseDTO>(x)) });
        }

        [HttpPost(), Route("createUser")]
        public IActionResult AddUser([FromForm] UserAddRequestDTO user, [FromHeader] String token)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return Unauthorized(new { success = false, message = "Invalid token", code = unauthorized });
            }
            var currentUser = Mapper.Map<User>(user);
            dbContext.Users.Add(currentUser);
            dbContext.SaveChanges();
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
            var currentUser = dbContext.Users.Where(x => x.Id == Id && x.IsDeleted == false).SingleOrDefault();
            if (currentUser == null)
            {
                return NotFound(new { success = false, message = "User Not Found", code = notFound });
            }
            Mapper.Map(user, currentUser);
            dbContext.SaveChanges();
            return Ok(new { success = true, message = "update successfull", code = success, user = currentUser });
        }

        [HttpPatch("{id}")]
        public IActionResult updateUserPartially(int id, [FromHeader] String token, JsonPatchDocument authorPatch)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return Unauthorized(new { success = false, message = "Invalid token", code = unauthorized });
            }
            var currentUser = dbContext.Users.Where(x => x.Id == id && x.IsDeleted == false).SingleOrDefault();
            if (currentUser == null)
            {
                return NotFound(new { success = false, message = "User Not Found", code = notFound });
            }
            authorPatch.ApplyTo(currentUser);
            return Ok(new { success = true, message = "update successfull", code = success, user = currentUser });
        }

        [HttpDelete(), Route("deleteUser/{Id}")]
        public IActionResult DeleteUser(int id, [FromHeader] String token)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return Unauthorized(new { success = false, message = "Invalid token", code = unauthorized });
            }
            var currentUser = dbContext.Users.Where(x => x.Id == id && x.IsDeleted == false).SingleOrDefault();
            if (currentUser == null)
            {
                return NotFound(new { success = false, message = "User Not Found", code = notFound });
            }
            var currentSkills = SkillsController.skills.Where(x => x == currentUser.Skills.FirstOrDefault()).FirstOrDefault();
            if (currentSkills != null)
            {
                SkillsController.skills.Remove(currentSkills);
            }
            currentUser.IsDeleted = true;
            dbContext.SaveChanges();
            return Ok(new { success = true, message = "delete successfull", code = success, user = currentUser });
        }

    }
}
