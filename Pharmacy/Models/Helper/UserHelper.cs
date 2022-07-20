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
                SkillsSize = (user.Skills == null) ? 0 : user.Skills.Count()
            };

        }
    }
}
