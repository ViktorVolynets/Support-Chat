using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalSupport.Utils
{
    public class AuthModel
    {
        [Required(ErrorMessage = "Empty Data")]
        public string UserString { get; set; } //represent user`s email/phone number data


        [Required]
        [DataType(DataType.Password)]
        [StringLength(30) , MinLength(6)]
        public string Password { get; set; }
    }
}
