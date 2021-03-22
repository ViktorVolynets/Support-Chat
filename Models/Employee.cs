using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace TechnicalSupport.Models
{
    public partial class Employee
    {
        public int EmployeeId { get; set; }
        public bool StatusOnline { get; set; }
        public int? Age { get; set; }

        [AllowNull]
        public virtual WorkTime WorkTime { get; set; }
      
        public Guid UserUserId { get; set; }
        public User User { get; set; }
    }
}
