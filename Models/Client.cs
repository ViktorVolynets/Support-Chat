using System;
using System.Collections.Generic;

#nullable disable

namespace TechnicalSupport.Models
{
    public partial class Client
    {
        public int ClientId { get; set; }
        public string SecondName { get; set; }
        public int? Age { get; set; }
        public string UserIp { get; set; }
        public Guid UserUserId { get; set; }
        public User User { get; set; }
    }
}
