using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TechnicalSupport.Data;
using TechnicalSupport.Models;

namespace TechnicalSupport.Controllers
{
    public class HomeController : Controller
    {
        public static ChatContext _context;
       
        public static Dictionary<string, string> useremail;
        public HomeController( IHubContext<MessageHub> hubContext)
        {
     //       _context = context;

        }

        public IActionResult Index()
        {
            return View();

           // return RedirectToAction("Index", "Account");

        }
 

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
