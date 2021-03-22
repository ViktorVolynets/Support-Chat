using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalSupport.Utils
{
    public class JoinModel
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "NameLengthError")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50 , MinimumLength = 3 , ErrorMessage = "NameLengthError")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [StringLength(50 , MinimumLength = 6 , ErrorMessage = "PasswordSizeError")]
        public string PasswordString { get; set; }

        public int Age { get; set; }
        public int Sex;

    }
}
