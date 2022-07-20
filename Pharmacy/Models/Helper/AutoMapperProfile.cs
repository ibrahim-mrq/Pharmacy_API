using AutoMapper;
using Pharmacy.Models.RequestDTO;
using Pharmacy.Models.ResponseDTO;
using System.Linq;

namespace Pharmacy.Models.Helper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserAddRequestDTO, User>().ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false));

            CreateMap<UserUpdateRequestDTO, User>();

            CreateMap<User, UserResponseDTO>()
                .ForMember(dest => dest.SkillsSize, opt => opt.MapFrom(src => src.Skills.Count()));
        }

    }
}
