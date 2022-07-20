using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public String Name { get; set; } = "";
        public String Email { get; set; } = "";
        public String Phone { get; set; } = "";
        public String Location { get; set; } = "";
        public int BarthDate { get; set; } = 0;
        public String Password { get; set; } = "";
        public List<Skills> Skills { get; set; } = new List<Skills>();
        public int SkillsSize { get; set; } = 0;
        public Boolean IsDeleted { get; set; }


    }
}
