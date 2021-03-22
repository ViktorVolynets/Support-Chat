using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalSupport.Utils
{
    public class JoinEmployeeModel : JoinModel
    {
        public bool canCreateEmployee { get; set; }

        public bool canModifyUser { get; set; }
        public bool canModifyEmployee { get; set; }
    }
}
