using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Pharmacy.Models;
using Pharmacy.Models.Helper;
using Pharmacy.Models.RequestDTO;
using Pharmacy.Models.ResponseDTO;
using Pharmacy.Repositories.Interfaces;
using System;
using System.Linq;
using System.Text;

namespace Pharmacy.Repositories
{
    public class UserRepository : IUserRepository
    {
        public IMapper mapper { get; }
        public DBContext dbContext { get; }

        public UserRepository(IMapper mapper, DBContext dbContext)
        {
            this.mapper = mapper;
            this.dbContext = dbContext;
        }

        public Response<UserResponseDTO> GetAll(IUrlHelper Url, PagingDTO Page)
        {
            var list = dbContext.Users.AsQueryable()
                .Where(x => x.IsDeleted == false)
                .Select(x => mapper.Map<UserResponseDTO>(x));

            var respone = new Response<UserResponseDTO>(list, Page)
            {
                success = true,
                message = "success",
                code = 200
            };
            if (respone.Pageing.hasNextPage)
            {
                respone.Pageing.NextPageUrl = Url.Link("getAllUsers", new { Page.RowCount, PageNumber = Page.PageNumber + 1 });
            }
            if (respone.Pageing.hasPrevPage)
            {
                respone.Pageing.PrevPageUrl = Url.Link("getAllUsers", new { Page.RowCount, PageNumber = Page.PageNumber - 1 });
            }
            return respone;
        }

        public UserResponseDTO GetById(int Id, String token)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return null;
            }
            var currentUser = dbContext.Users.Where(x => x.Id == Id && x.IsDeleted == false).SingleOrDefault();
            if (currentUser == null)
            {
                return null;
            }
            return mapper.Map<UserResponseDTO>(currentUser);
        }

        public Response<UserResponseDTO> Search(string Name)
        {
            var query = dbContext.Users.AsQueryable()
                .Where(x => x.IsDeleted == false)
                .Select(x => mapper.Map<UserResponseDTO>(x));

            if (!String.IsNullOrWhiteSpace(Name))
            {
                query = query.Where(x => x.Name.Contains(Name, StringComparison.OrdinalIgnoreCase));
            }
            query = query.OrderBy(x => x.Name).ThenBy(x => x.Location);

            var respone = new Response<UserResponseDTO>(query, new PagingDTO { RowCount = 1, PageNumber = 1 })
            {
                success = true,
                message = "success",
                code = 200
            };
            //  return mapper.Map<UserResponseDTO>(query);
            return respone;
        }

        public User Create(UserAddRequestDTO user, string token)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return null;
            }
            var currentUser = mapper.Map<User>(user);
            byte[] hash, salt;
            GenerateHash(user.Password, out hash, out salt);
            currentUser.PasswordHash = hash;
            currentUser.PasswordSalt = salt;

            currentUser = dbContext.Users.Add(currentUser).Entity;
            SaveChangess();
            return currentUser;
        }

        public User login(UserAddRequestDTO user)
        {
            var currentUser = dbContext.Users.Where(x => x.Equals(user.Email)).SingleOrDefault();
            if (currentUser == null)
            {
                return null;
            }
            if (ValidateHash(user.Password, currentUser.PasswordHash, currentUser.PasswordSalt))
            {
                return null;
            }
            return currentUser;
        }

        public User Update(int Id, UserUpdateRequestDTO user, string token)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                // return Unauthorized(new { success = false, message = "Invalid token", code = unauthorized });
                return null;
            }
            if (String.IsNullOrWhiteSpace(user.Name))
            {
                //   return BadRequest(new { success = false, message = "Invalid Empty Name", code = invalidValue });
                return null;
            }
            var currentUser = dbContext.Users.Where(x => x.Id == Id && x.IsDeleted == false).SingleOrDefault();
            if (currentUser == null)
            {
                // return NotFound(new { success = false, message = "User Not Found", code = notFound });
                return null;
            }
            mapper.Map(user, currentUser);
            SaveChangess();
            return currentUser;
        }

        public User Delete(int Id, string token)
        {
            if (String.IsNullOrWhiteSpace(token))
            {
                return null;
            }
            var currentUser = dbContext.Users.Where(x => x.Id == Id && x.IsDeleted == false).SingleOrDefault();
            if (currentUser == null)
            {
                return null;
            }
            var currentSkills = dbContext.Skills.Where(x => x == currentUser.Skills.FirstOrDefault()).FirstOrDefault();
            if (currentSkills != null)
            {
                dbContext.Skills.Remove(currentSkills);
            }
            currentUser.IsDeleted = true;
            dbContext.SaveChanges();
            return currentUser;
        }

        public void GenerateHash(String password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hash = new System.Security.Cryptography.HMACSHA512())
            {
                passwordHash = hash.ComputeHash(Encoding.UTF8.GetBytes(password));
                passwordSalt = hash.Key;
            }
        }
        private Boolean ValidateHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hash = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var newPassHash = hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < newPassHash.Length; i++)
                {
                    if (newPassHash[i] != passwordHash[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }
        public int SaveChangess()
        {
            return dbContext.SaveChanges();
        }
    }

}
