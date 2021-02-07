using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalSupport.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }

        public string SenderType {get; set;}

        public string RedirectTo { get; set; }
        public Guid ClientId { set; get; }
        public Guid DialogId { get; set; }
        //
        public Dialog Dialog { get; set; }
    }

}
