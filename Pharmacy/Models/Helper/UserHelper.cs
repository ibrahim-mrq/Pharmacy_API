using Pharmacy.Models.ResponseDTO;
using System.Linq;

namespace Pharmacy.Models.Helper
{
    public class UserHelper
    {
        public static UserResponseDTO MapDomainToResponse(User user)
        {
            return new UserResponseDTO()
            {
                Id = user.Id,
                Name = user.Name,
                Location = user.Location,
                Skills = user.Skills,
                Phone = user.Phone,
                BarthDate = user.BarthDate,
                Email= user.Email,
                Password = user.Password,
                SkillsSize = (user.Skills == null) ? 0 : user.Skills.Count()
            };

        }
    }
}
