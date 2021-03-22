using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TechnicalSupport.Services
{
    public interface ICryptoProvider
    {
        /// <summary>
        /// Method for obtaining password hash
        /// </summary>
        /// <param name="str_password">User password</param>
        /// <param name="l_salt">Local password salt</param>
        /// <returns>Password hash</returns>
        public byte[] GetPasswordHash(string str_password, byte[] l_salt);

        /// <summary>
        /// Generates specified length random hash
        /// </summary>
        public byte[] GetRandomSaltString();

        /// <summary>
        /// Applies a hash algorithm to a given array
        /// </summary>
        /// <param name="target">Target byte array</param>
        /// <returns>Hashed byte array</returns>
        public byte[] GetSHA256Hash(byte[] target);

        /// <summary>
        /// Method for token transfering for further comparison. 
        /// </summary>
        /// <param name="str_token">Admin-creation token</param>
        /// <returns>Returns a token as an array</returns>
        public byte[] GetTokenBytes(string str_token);
    }
}
