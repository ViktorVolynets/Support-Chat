using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalSupport.Models
{
    public class User
    { 
        public Guid UserId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public int? RoleId { get; set; }
        public Role Role { get; set; }


        public string Email { get; set; }
        public string Phone { get; set; }


        [Display(Name = "Password")]
        public byte[] PasswordHash { get; set; }
        public byte[] LocalHash { get; set; }
    }
}
