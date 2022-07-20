using System;
using System.Collections.Generic;

namespace Pharmacy.Models.ResponseDTO
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Email { get; set; }
        public String Phone { get; set; } 
        public String Location { get; set; }
        public int BarthDate { get; set; }
        public String Password { get; set; }
        public List<Skills> Skills { get; set; }
        public int SkillsSize { get; set; }
    }
}
