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

namespace TechnicalSupport.Models
{


    public class CustomUserIdProvider : IUserIdProvider
    {

   
      
        public virtual string GetUserId(HubConnectionContext connection)
        {

            if (connection.User?.Identity.Name != null)
            {

                var userDichtionary = HomeController.useremail;

                // string t = userDichtionary.Keys.FirstOrDefault(s => s.Contains(connection.User?.Identity.Name));
                // string t = MessageHub._context.Users.FirstOrDefault(s => s.Email == connection.User.Identity.Name.ToString()).Id.ToString();
                // string t = userDichtionary.Where((d, v) => d.Key.Contains(connection.User.Identity.Name.ToString())).Select(v => v.Values).ToList();
                string t = userDichtionary[connection.User?.Identity.Name].ToString();

                return t;

            }


            else return Guid.NewGuid().ToString();
            // или так
            //return connection.User?.FindFirst(ClaimTypes.Name)?.Value;
        }




    }
}