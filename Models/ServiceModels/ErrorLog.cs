using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalSupport.Models.ServiceModels
{
    public class ErrorLog
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public string LogMessage { get; set; }
        public int Code { get; set; }
        public string StackTrace { get; set; }
    }
}
