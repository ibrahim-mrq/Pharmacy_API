using System;
using System.ComponentModel.DataAnnotations;

namespace Pharmacy.Models.RequestDTO
{
    public class UserAddRequestDTO
    {
        [Required]
     //   [StringLength(10)]
        public String Name { get; set; }
        [Required(ErrorMessage = "Invalid Empty Name")]
        [EmailAddress]  
        public String Email { get; set; }
        [Required]
        [Phone]
        public String Phone { get; set; }
        [Required]
        public String Location { get; set; }
        [Range(minimum: 1990, maximum: 2010)]
        public int BarthDate { get; set; }
        [Required]
        public String Password { get; set; }
        [Required]
        [Compare("Password")]
        public String ConfirmPassword { get; set; }
    }
}
