using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalSupport.Data;
using TechnicalSupport.Models;

namespace TechnicalSupport.AutoChat
{
    public class DialogEmployee 
    {
        private ChatContext _context;
        public DialogEmployee(ChatContext context)
        {
           _context = context;

        }
        public Message DialogEmployeeMain(Message mes, ref Dictionary<Guid, int> clientState)
        {
            var emp = _context.Employees.Where(w => w.StatusOnline == true).FirstOrDefault();

            if (emp != null)
            {
                Dialog thisDialog = _context.Dialogs.Where(w => w.DialogId == mes.DialogId).FirstOrDefault();
                thisDialog.EmployeeUserUserId = emp.UserUserId;
                _context.SaveChanges();
                mes.Text = $"Перемикаю на оператора {emp.User.FirstName}";
            }
            else
            {
                mes.Text = "На даний момент немає доступних працівників";
                clientState[mes.DialogId] = 0;

            }
            return mes;
        }


    }
}
