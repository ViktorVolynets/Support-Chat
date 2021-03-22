using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalSupport.Utils
{
    public class JoinAdminModel : JoinModel
    {
        [Required]
        public string AccessToken { get; set; }
    }
}
