using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TechnicalSupport.Data;
using TechnicalSupport.Models;
using TechnicalSupport.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using support_chat.Utils;
using Microsoft.Extensions.Logging;

namespace TechnicalSupport.Services
{

    public class AuthService : IAuthService
    {

        private ChatContext _db;
        private readonly CryptoProvider _cryProvider;
        private readonly IHttpContextAccessor _contextAcessor;
        private readonly ILogger _logger;
        private AuthStatusResult _authResult;
        


        public AuthService(ChatContext dbContext , ICryptoProvider cryptoProvider,
            IHttpContextAccessor contextAccessor, ILogger<AuthService> logger)
        {
            _db = dbContext;
            _cryProvider = (CryptoProvider)cryptoProvider;
            _contextAcessor = contextAccessor;
            _logger = logger;
            _authResult = new AuthStatusResult();

        }


        public Task<AuthStatusResult> AuthenticateUserAsync(AuthModel model)
        {
            return Task.Run(() => AuthenticateUser(model));
        }


        private async Task<AuthStatusResult> AuthenticateUser(AuthModel model)
        {

            var user = await _db.Users.SingleOrDefaultAsync(x => x.Phone == model.UserString || x.Email == model.UserString);

            //If user write email/phone that doesnt exist in db
            if(user == null)
            {
                _authResult.IncorrectData = true;
                _authResult.isSuccessful = false;

                return _authResult;
            }

            var enteredPassword = _cryProvider.GetPasswordHash(model.Password, user.LocalHash);
            //If password is incorrect
            if( !user.PasswordHash.SequenceEqual(enteredPassword))
            {
                _authResult.IncorrectPassword = true;
                _authResult.isSuccessful = false;

                return _authResult;
            }
            
            //all credentials are true
            List<Claim> userClaims = await VerifyUserAsync(user);

            if(userClaims == null)
            {
                _authResult.IncorrectData = true;
                _authResult.isSuccessful = false;

                return _authResult;
            }

            var u_id = new ClaimsIdentity(userClaims, "ApplicationCookie");
            var claimsPrincipal = new ClaimsPrincipal(u_id);

            await AuthenticationHttpContextExtensions.SignInAsync(_contextAcessor.HttpContext, claimsPrincipal);
            _logger.LogInformation($"User email: {user.Email} has been authenticated");

            return _authResult;
        }


        private Task< List<Claim> > VerifyUserAsync(User user)
        {
            return Task.Run(() => VerifyUser(user));
        }


        private async Task< List<Claim>> VerifyUser(User user)
        {
            switch (user.RoleId)
            {
                case 1:
                    return await CreateClientClaims(user);
                case 2:
                    return await CreateEmployeeClaims(user);
                case 3:
                    return await CreateAdminClaims(user);
                default:
                    return null;
            }
        }


        private async Task<List<Claim>> CreateClientClaims(User user)
        {
            var client = await _db.Users.SingleOrDefaultAsync(x => x.UserId == user.UserId);

            if(client == null)
            {
                _logger.LogWarning($"Not found entity in client table with email: {user.Email}");

                return null;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType , client.FirstName + client.LastName),
                new Claim(ClaimTypes.Role , nameof(client).ToUpper())
            };

            return claims;
        }
        private async Task<List<Claim>> CreateEmployeeClaims(User user)
        {
            var employee = await _db.Employees.Include(u=>u.User).SingleOrDefaultAsync(x => x.UserUserId == user.UserId);

            if(employee == null)
            {
                _logger.LogWarning($"Not found entity in employee table with email: {user.Email}");

                return null;
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType , employee.User.FirstName + employee.User.LastName),
                new Claim(ClaimTypes.Role , nameof(employee).ToUpper())
            };

            return claims;
        }
        

        private async Task<List<Claim>> CreateAdminClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType , user.Email),
                new Claim(ClaimTypes.Role , "ADMIN")
            };

            return claims;
        }


        public Task SignOutAsync()
        {
            return Task.Run(() => SignOut());
        }

        
        private async Task SignOut()
        {
            await AuthenticationHttpContextExtensions.SignOutAsync(_contextAcessor.HttpContext);
        }
    }
}
