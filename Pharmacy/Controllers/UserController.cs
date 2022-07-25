using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Models;
using Pharmacy.Models.Helper;
using Pharmacy.Models.RequestDTO;
using Pharmacy.Models.ResponseDTO;
using Pharmacy.Repositories.Interfaces;
using System;
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
        private readonly IUserRepository userRepository;

        public UserController(IMapper mapper, DBContext DBContext, IUserRepository userRepository)
        {
            Mapper = mapper;
            this.dbContext = DBContext;
            this.userRepository = userRepository;
        }

        [HttpGet(nameof(GetAllUsers), Name = "getAllUsers"), Route("getAllUsers")]
        public IActionResult GetAllUsers([FromQuery] PagingDTO Page)
        {
            var respone = userRepository.GetAll(Url, Page);
            return Ok(respone);
        }

        [HttpGet(), Route("getUserById/{Id}")]
        public IActionResult GetUserById([FromRoute] int Id, [FromHeader] String token)
        {
            var respone = userRepository.GetById(Id, token);
            if (respone == null)
            {
                return BadRequest(new { success = false, message = "user not found", code = invalidValue });
            }
            return Ok(new { success = true, message = "success", code = success, user = respone });
        }

        [HttpGet(), Route("searchUser")]
        public IActionResult SearchUser([FromQuery] String Name)
        {
            var respone = userRepository.Search(Name);
            if (respone == null)
            {
                return BadRequest(new { success = false, message = "user not found", code = invalidValue });
            }
            return Ok(new { success = true, message = "success", code = success, user = respone });
        }

        [HttpPost(), Route("createUser")]
        public IActionResult AddUser([FromForm] UserAddRequestDTO user, [FromHeader] String token)
        {
            var respone = userRepository.Create(user, token);
            if (respone == null)
            {
                return BadRequest(new { success = false, message = "user not found", code = invalidValue });
            }
            return CreatedAtAction(nameof(GetUserById), new { Id = respone.Id },
                new { success = true, message = "Created", code = created, user = UserHelper.MapDomainToResponse(respone) });
        }

        [HttpPut(), Route("updateUser/{Id}")]
        public IActionResult UpdateUser([FromRoute] int Id, [FromForm] UserUpdateRequestDTO user, [FromHeader] String token)
        {
            var respone = userRepository.Update(Id, user, token);
            if (respone == null)
            {
                return BadRequest(new { success = false, message = "user not found", code = invalidValue });
            }
            return Ok(new { success = true, message = "update successfull", code = success, user = respone });
        }

        [HttpDelete(), Route("deleteUser/{Id}")]
        public IActionResult DeleteUser(int id, [FromHeader] String token)
        {
            var respone = userRepository.Delete(id, token);
            if (respone == null)
            {
                return BadRequest(new { success = false, message = "user not found", code = invalidValue });
            }
            return Ok(new { success = true, message = "delete successfull", code = success, user = respone });
        }

    }
}
