using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pharmacy.Models
{
    public class User
    {
        [Key]
    //    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public String Name { get; set; } = "";
        [Required]
        [EmailAddress]
        public String Email { get; set; } = "";
        [Required]
        [Phone]
        public String Phone { get; set; } = "";
        [Required]
        public String Location { get; set; } = "";
        [Required]
        public String BarthDate { get; set; } = "";
        [Required]
        public String Password { get; set; } = "";
        public List<Skills> Skills { get; set; } = new List<Skills>();
        public int SkillsSize { get; set; } = 0;
        public Boolean IsDeleted { get; set; }


    }
}
