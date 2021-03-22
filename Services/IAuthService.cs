using Microsoft.AspNetCore.Mvc;
using TechnicalSupport.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalSupport.Models;
using support_chat.Utils;

namespace TechnicalSupport.Services
{
    public interface IAuthService
    {

        /// <summary>
        /// Asynchronous method for cookie-based user authentication
        /// </summary>
        /// <param name="model">Model that includes user credentials</param>
        /// <returns>AuthResult object which describe errors if there`re any</returns>
        Task<AuthStatusResult> AuthenticateUserAsync(AuthModel model);

        /// <summary>
        /// Asynchronous method that clear user`s auth cookie
        /// </summary>
        /// <returns></returns>
        Task SignOutAsync();

    }
}
