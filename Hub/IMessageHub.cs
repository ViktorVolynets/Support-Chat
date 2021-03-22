using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalSupport.Models;

namespace TechnicalSupport.Hub
{
        public interface IMessageHub
        {
            Task Receive(Message message);
 
        }
    
}
