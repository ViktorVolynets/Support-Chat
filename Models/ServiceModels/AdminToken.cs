using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalSupport.Models.ServiceModels
{
    public class AdminToken
    {
        public int AdminTokenId { get; set; }
        [Required]
        public byte[] TokenData { get; set; }
        public bool isUsed { get; set; }
    }
}
