using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace support_chat.Utils

{
    public class AuthStatusResult
    {
        public bool isSuccessful { get; set; }
        public string RoleName { get; set; }
        public bool IncorrectPassword { get; set; }
        public bool IncorrectData { get; set; }


        public AuthStatusResult()
        {
            isSuccessful = true;
            IncorrectData = false;
            IncorrectPassword = false;
        }
    }
}
