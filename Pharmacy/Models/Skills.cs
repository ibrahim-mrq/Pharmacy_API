using System;

namespace Pharmacy.Models
{
    public class Skills
    {

        public int Id { get; set; }
        public String Name { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}
