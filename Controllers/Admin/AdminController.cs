using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalSupport.Data;
using TechnicalSupport.Models;
using TechnicalSupport.Services;
using TechnicalSupport.Utils;

namespace TechnicalSupport.Controllers.Admin
{
    [Authorize(Roles = "ADMIN")]
    public class AdminController : Controller
    {
        private readonly ChatContext _db;
        private readonly IJoinService _joinService;
        private readonly IAdminServiceProvider _adminService;
        private readonly IHubContext<MessageHub> _hubContext;

        public AdminController(
            ChatContext context, IJoinService joinService,
            IAdminServiceProvider adminService , IHubContext<MessageHub> hubContext)
        {

            _db = context;
            _joinService = joinService;
            _adminService = adminService;
            _hubContext = hubContext;

        }


        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> GetCurrentChatSessions()
        {
            return Ok();
        }

        //
        [HttpGet]
        public async Task<IActionResult> Clients()
        {
            ViewBag.Clients = await _adminService.GetClientListAsync();

            return View("Views/Admin/Clients/Clients.cshtml");
        }


        [HttpGet]
        public async Task<IActionResult> ChangeClient(int id)
        {
            ViewBag.Client = await _db.Clients.Include(x => x.User).SingleOrDefaultAsync(x => x.ClientId == id);

            return View("Views/Admin/Clients/ChangeClient.cshtml");
        }


        [HttpPost]
        public async Task<IActionResult> ChangeClient(Client client)
        {
            if (ModelState.IsValid)
            {
                await _adminService.ChangeClientAsync(client);
            }

            return RedirectToAction(nameof(Clients));
            
        }


        [HttpGet]
        public async Task<IActionResult> Employees()
        {
            ViewBag.Employees = await _adminService.GetEmployeeListAsync();

            return View("Views/Admin/Employees/Employees.cshtml");
        }


        [HttpGet]
        public async Task<IActionResult> ChangeEmployee(int id)
        {
            ViewBag.Employee = await _db.Employees.Include(x => x.User).SingleOrDefaultAsync(x => x.EmployeeId == id);

            return View("Views/Admin/Employees/ChangeEmployee.cshtml");
        }


        [HttpPost]
        public async Task<IActionResult> ChangeEmployee(Employee employee)
        {
            if (ModelState.IsValid)
            {
                await _adminService.ChangeEmployeeAsync(employee);

            }

            return RedirectToAction(nameof(Employees));
        }


        [HttpGet]
        public IActionResult CreateEmployee()
        {

            return View("Views/Admin/Employees/CreateEmployee.cshtml");

        }



        [HttpPost]
        public async Task<IActionResult> CreateEmployee(JoinEmployeeModel model)
        {

            if (ModelState.IsValid)
            {

                await _adminService.CreateEmployeeAsync(model);

            }

            return RedirectToAction(nameof(Employees));
        }


        [HttpGet]
        public IActionResult CreateAdmin()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> CreateAdmin(JoinAdminModel model)
        {
            if (ModelState.IsValid)
            {
                await _adminService.CreateAdminAsync(model);
            }

            return RedirectToAction(nameof(Index));
        }

        
        [HttpGet]
        public async Task<IActionResult> CreateTokens()
        {
            await _adminService.CreateTokensAsync();

            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task <IActionResult> Stats()
        {
            ViewData["ActiveUsers"] = "NOT EMPLEMENTED";
            ViewData["ActiveOperators"] = await _adminService.GetActiveOperatorsAsync();
            return View("Views/Admin/Stats/Stats.cshtml");
        }

        [HttpGet]
        public IActionResult ErrorLogs()
        {
            ViewBag.ErrorLogs = _adminService.GetErrorLogs();

            return View("Views/Admin/Stats/ErrorLogs.cshtml");
        }

        [HttpGet]
        public async Task<IActionResult> GetErrorLog(int id)
        {
            ViewBag.Log = await _adminService.GetErrorLogAsync(id);
            return View("Views/Admin/Stats/LogInfo.cshtml");
        }

        [HttpGet]
        public IActionResult TraceLogs()
        {
            ViewBag.TraceLogs = _adminService.GetTraceLogs();

            return View("Views/Admin/Stats/TraceLogs.cshtml");
        }
    }
}
