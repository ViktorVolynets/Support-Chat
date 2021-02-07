using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading;
using System.Security.Principal;
using TechnicalSupport.Data;
using Microsoft.EntityFrameworkCore;
using TechnicalSupport.Models;

namespace TechnicalSupport
{
    public class MessageHub : Hub
    {
        private ChatContext _context;

       
        public MessageHub (ChatContext context)
        {
            _context = context;

           
        }


        // [Authorize]
        public async Task Send(string message, string userName)
        {


            if (!Context.User.Identity.IsAuthenticated)
            {
                await Clients.Group(Context.UserIdentifier).SendAsync("Receive", Context.UserIdentifier.ToString() + "grup", Context.UserIdentifier);
                var empid = _context.Dialogs.FirstOrDefault(em => em.UserId.ToString() == Context.UserIdentifier).EmployeeId;
                   
            if (empid!= null)
                {
                   
                    await Clients.User(empid.ToString()).SendAsync("Receive", empid.ToString() + "empoly", empid);

                }


            }





            if (Context.User.Identity.IsAuthenticated)
               
            {
               
                    await Clients.User(Context.UserIdentifier.ToString()).SendAsync("Receive", Context.UserIdentifier.ToString() + "send", Context.UserIdentifier);


                IReadOnlyList<string> emp = _context.Dialogs.Where(em => em.EmployeeId.ToString() == Context.UserIdentifier).Select(s=>s.UserId.ToString()).ToList<string>();

                if (emp != null)
                {

                    foreach (var t in emp)
                    {
                        await Clients.Group(t.ToLower()).SendAsync("Receive", t.ToString() + "grup", Context.UserIdentifier);
                      

                    }
                 
                }



            }


            //await Clients.All.SendAsync("Receive", message, userName);
        }




        [Authorize(Roles = "admin")]
        public async Task Notify(string message, string userName)
        {
            if (Context.User.Identity.IsAuthenticated)

            {
                //Employee emp = await _context.Employees
                //   .Include(u => u.Role)
                //   .SingleOrDefaultAsync(u => u.Email == Context.User.Identity.Name);


                await Clients.User(Context.UserIdentifier.ToString()).SendAsync("Receive", Context.UserIdentifier.ToString() + "send", Context.UserIdentifier);
                var emp = _context.Dialogs.Find(Context.UserIdentifier);
                if (emp != null)
                {

                    await Clients.Group(emp.UserId.ToString()).SendAsync("Receive", emp.UserId.ToString() + "empoly", emp.UserId.ToString());

                }



            }
        }

       
        public override async Task OnConnectedAsync()
        {

            if (Context.User.Identity.IsAuthenticated)
            {
                Employee employee = await _context.Employees
                  .Include(u => u.Role)
                  .SingleOrDefaultAsync(u => u.Email == Context.User.Identity.Name);

                if (employee != null)
                {
                    employee.StatusOnline = true;
                    await Clients.User(employee.Id.ToString()).SendAsync("Receive", employee.Id.ToString() + "send", employee.Name.ToString());

                }




            } else
            {
                Guid Id = Guid.Parse(Context.UserIdentifier); 
                  

                await Groups.AddToGroupAsync(Context.ConnectionId, Id.ToString());

                _context.Users.Add(new User() { Id = Id });


                Employee name = _context.Employees.Where(e => e.StatusOnline == true).FirstOrDefault();

                if (name!=null)
                {

                     _context.Dialogs.Add(new Dialog() { UserId = Id, DialogId = Guid.NewGuid(), EmployeeId = name.Id });

                }


            }
             

            _context.SaveChanges();




            await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} вошел в чат");
            await base.OnConnectedAsync();
        }
      
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Clients.All.SendAsync("Notify", $"{Context.ConnectionId} покинул в чат");
            await base.OnDisconnectedAsync(exception);
        }

    }
}
