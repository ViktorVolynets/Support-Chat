using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalSupport.Models
{
    public class Dialog
    {
        public Guid DialogId { get; set; }
        //
        public Guid ClientUserUserId { get; set; }
        public Client Client { get; set; }
        public Guid EmployeeUserUserId { get; set; }
        public Employee Employee { get; set; }

       
    }
}
