using System;
using System.Collections.Generic;

namespace Pharmacy.Models.ResponseDTO
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Location { get; set; }
        public List<Skills> Skills { get; set; } = new List<Skills>();
        public int SkillsSize { get; set; }
    }
}
