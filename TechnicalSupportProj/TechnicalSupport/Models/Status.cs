using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalSupport.Models
{
    public class Status
    {
        public int Id { get; set; }
        public string State { get; set; }
        public Guid DialogId { get; set; }
        //
        public Dialog Dialog { get; set; }

    }
}
