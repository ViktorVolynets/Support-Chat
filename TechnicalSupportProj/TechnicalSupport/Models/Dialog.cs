using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalSupport.Models
{
    public class Dialog
    {
        public Guid DialogId { get; set; }
        public Guid UserId { get; set; }
        public Guid EmployeeId { get; set; }
        //
        public User User { get; set; }
        public Employee Employee { get; set; }
        public Status Status { get; set; }
       
    }
}
