using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using support_chat.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TechnicalSupport.Data;
using TechnicalSupport.Services;
using TechnicalSupport.Utils;

namespace TechnicalSupport.Controllers
{

 
    public class AccountController : Controller
    {
        private readonly ChatContext _db;
        private readonly IAuthService _authService;
        private readonly IJoinService _joinService;

        public AccountController(ChatContext db , IAuthService authService , IJoinService joinService)
        {
            _db = db;
            _authService = authService;
            _joinService = joinService;
        }


        public IActionResult Index()
        {
            if(!User.HasClaim( x => x.Type == ClaimTypes.Role))
            {
                return RedirectToAction(nameof(Login));
            }

            return View();
        }


        //Registration
        [HttpGet]
        public IActionResult Join()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Join(JoinModel model)
        {
            if(await _joinService.canJoin(model))
            {
                await _joinService.JoinClient(model);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewData["canJoin"] = false;
                return View();
            }
            
        }


        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.AuthResult = new AuthStatusResult();
            return View();
        }


        
        [HttpPost]
        public async Task<IActionResult> Login(AuthModel model)
        {
            var lResult = await _authService.AuthenticateUserAsync(model);
            if (lResult.isSuccessful == false)
            {

                ViewBag.AuthResult = lResult;
                return View(model);

            }
            else
            {
                return RedirectToAction(nameof(Index), "Account");

            }
            
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _authService.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Employee()
        {
            return View();
        }
    }
}
