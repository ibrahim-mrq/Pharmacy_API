using Microsoft.AspNetCore.Mvc;
using Pharmacy.Models;
using Pharmacy.Models.Helper;
using Pharmacy.Models.RequestDTO;
using Pharmacy.Models.ResponseDTO;
using System;

namespace Pharmacy.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Response<UserResponseDTO> GetAll(IUrlHelper Url, PagingDTO Page);
        UserResponseDTO GetById(int Id, String token);
        Response<UserResponseDTO> Search(String Name);
        User Create(UserAddRequestDTO user, String token);
        User Update(int Id, UserUpdateRequestDTO user, String token);
        User Delete(int Id, String token);
        int SaveChangess();
    }
}
