using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using TechnicalSupport.Data;
using TechnicalSupport.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace TechnicalSupport.Models
{


    public class CustomUserIdProvider : IUserIdProvider
    {
        private  ChatContext _chatContext;
        private  Dictionary<string, User> usersDictionar;

        public CustomUserIdProvider (ChatContext db)
        {     
            _chatContext = db;
            usersDictionar = _chatContext?.Users?.Where(s=>s.FirstName != null).ToDictionary(s => s.FirstName+s.LastName);
        }


        public virtual string GetUserId(HubConnectionContext connection)
        {

            if (connection.User?.Identity.Name != null && usersDictionar.ContainsKey(connection.User.Identity.Name))
            {
                return usersDictionar[connection.User.Identity.Name].UserId.ToString();  
            }
            else return Guid.NewGuid().ToString();
          
        }

    }
}