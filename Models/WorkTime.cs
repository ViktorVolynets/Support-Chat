using System;
using System.Collections.Generic;

#nullable disable

namespace TechnicalSupport.Models
{
    public partial class WorkTime
    {
        public int Id { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
