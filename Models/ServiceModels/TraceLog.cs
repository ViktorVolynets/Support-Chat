using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalSupport.Models.ServiceModels
{
    public class TraceLog
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string LogMessage { get; set; }
    }
}
