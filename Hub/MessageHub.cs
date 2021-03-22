using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Security.Principal;
using TechnicalSupport.Data;
using Microsoft.EntityFrameworkCore;
using TechnicalSupport.Models;
using TechnicalSupport.Hub;

namespace TechnicalSupport
{
    public class MessageHub : Hub<IMessageHub>
    {
        ChatContext _context;
        AutoDialog _autoDialog;
        Dictionary<Guid, Dialog> _usersDialog;
        private readonly Guid _botId = default(Guid);
        public MessageHub(ChatContext context, AutoDialog auto)
        {
            _context = context;
            _autoDialog = auto;
            _usersDialog = new Dictionary<Guid, Dialog>();
            _usersDialog = _context.Dialogs?.ToDictionary(k => k.DialogId);
          
        }


        public async Task Send(Message message)
        {
            var dialog = _usersDialog
                .FirstOrDefault(x => x.Value.ClientUserUserId
                .ToString() == Context.UserIdentifier || x.Value.EmployeeUserUserId.ToString() == Context.UserIdentifier)
                .Value;


            if (dialog != null)
            {
                message.SenderType = "in";
                message.DialogId = dialog.DialogId;
                message.ClientId = dialog.ClientUserUserId;

                await Clients.User(Context.UserIdentifier).Receive(message);


                        if (dialog.EmployeeUserUserId == _botId)
                        {
                            await Clients.User(Context.UserIdentifier.ToString()).Receive(_autoDialog.ReplyMessage(message));
                        }
                        else
                        {
                            message.SenderType = "out";
                            await Clients.User(dialog.EmployeeUserUserId.ToString()).Receive(message);
                        }

            }
            else
            {

                Guid dialogId = Guid.NewGuid();

                Dialog newdialog = new Dialog() { 
                    ClientUserUserId = Guid.Parse(Context.UserIdentifier), 
                    DialogId = dialogId, 
                    EmployeeUserUserId = _botId 
                };

                _context.Dialogs.Add(newdialog);
                _usersDialog.Add(key: newdialog.DialogId, value: newdialog);
                _context.SaveChanges();

                message.DialogId = dialogId;
              
                await Clients.User(Context.UserIdentifier.ToString()).Receive(message);
                await Clients.User(Context.UserIdentifier.ToString()).Receive(_autoDialog.ReplyMessage(message));

            }

        }

        [Authorize(Roles = "EMPLOYEE")]
        public async Task SendAdmin (Message message)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                await Clients.User(Context.UserIdentifier.ToString()).Receive(message);
               
                if (_usersDialog.ContainsKey(message.DialogId))
                {
                    message.SenderType = "out";
                    await Clients.User(_usersDialog[message.DialogId].ClientUserUserId.ToString()).Receive(message);
                }
            }
        }
   
        public override async Task OnConnectedAsync()
        {
            
            if (Context.User.Identity.IsAuthenticated)
            {
                
                    var user = await _context.Users.Include(i => i.Role)
                        .FirstOrDefaultAsync(u => u.FirstName + u.LastName == Context.User.Identity.Name);

                    if (user != null)
                    {
                        var mes = new Message() { Name = user.FirstName, Text = $"Hello {user.FirstName}" };


                            if (user.Role.Name == "EMPLOYEE")
                            {
                             _context.Employees.FirstOrDefault(f => f.UserUserId == user.UserId).StatusOnline = true;
                            }
                            else
                            {
                                var dialog = _usersDialog
                                .FirstOrDefault(x => x.Value.ClientUserUserId.ToString() == Context.UserIdentifier || x.Value.EmployeeUserUserId
                                .ToString() == Context.UserIdentifier)
                                .Value;

                                Dialog dialogResalt;

                                if (dialog == null)
                                {
                                     dialogResalt = new Dialog() {
                                         ClientUserUserId = Guid.Parse(Context.UserIdentifier),
                                         DialogId = Guid.NewGuid(), 
                                         EmployeeUserUserId = _botId 
                                     };

                                    _context.Dialogs.Add(dialogResalt);
                                    _usersDialog.Add(key: dialogResalt.DialogId, value: dialogResalt);
                                }
                                else
                                {
                                    dialogResalt = dialog;
                                } 
                                mes.DialogId = dialogResalt.DialogId;
                            }

                        await Clients.User(Context.UserIdentifier.ToString()).Receive( mes);
                    }
           
            }
            else
            {
                Guid Id = Guid.Parse(Context.UserIdentifier);
             
                Guid dialogId = Guid.NewGuid();
                Dialog dialog = new Dialog() { 
                    ClientUserUserId = Id, 
                    DialogId = dialogId,
                    EmployeeUserUserId = _botId 
                };

                _context.Dialogs.Add(dialog);
                _usersDialog.Add(key: dialog.DialogId, value: dialog);

                Message mes = new Message() {
                    Name = "UserDefault",
                    Text = "Hello User",
                    DialogId = dialogId
                };

                await Clients.User(Id.ToString()).Receive(mes);

            }
            _context.SaveChanges();
            await base.OnConnectedAsync();
        }
      
        public override async Task OnDisconnectedAsync(Exception exception)
        {

                User user = await _context.Users.Include(i=>i.Role)
                .FirstOrDefaultAsync(u => u.UserId == Guid.Parse(Context.UserIdentifier));

                var dialog = _context.Dialogs
                .FirstOrDefault(f=>f.ClientUserUserId == Guid.Parse(Context.UserIdentifier)
                || f.EmployeeUserUserId == Guid.Parse(Context.UserIdentifier));
              

                if (dialog != null)
                {
                        var userOnline = dialog.ClientUserUserId.ToString() == Context.UserIdentifier ? dialog.EmployeeUserUserId : dialog.ClientUserUserId;

                       if(userOnline != _botId)
                        {
                          await Clients.User(userOnline.ToString()).Receive(new Message() { Text = "Disconected", DialogId = dialog.DialogId });
                        }
                  
                        _context.Remove(dialog);
                        _usersDialog.Remove(dialog.DialogId);
                }

             
                if (user != null && user.Role.Name == "EMPLOYEE")
                {
                _context.Employees.FirstOrDefault(f => f.UserUserId == user.UserId).StatusOnline = false;
                }

            _context.SaveChanges();
        
            await base.OnDisconnectedAsync(exception);
        }

    }
}
