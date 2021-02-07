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
        public HomeController(ChatContext context, IHubContext<MessageHub> hubContext)
        {
            _context = context;

            useremail = new Dictionary<string, string>();
            foreach (var t in _context.Employees)
            {
                useremail.Add(t.Email, t.Id.ToString());

            }
             

        }

        public IActionResult Index()
        {
            return View();
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
