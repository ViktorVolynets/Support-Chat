using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using TechnicalSupport.AutoChat;
using TechnicalSupport.Data;
using TechnicalSupport.Models;

namespace TechnicalSupport
{
    public class AutoDialog
    {
        delegate Message DialogMetod (Message mes, ref Dictionary<Guid, int> state);

        protected Dictionary<Guid, int> clientState;

        public static Dictionary<Guid, Dictionary<string, string>> UserDataDictionary;

        private ChatContext _context;

        enum Dialogs
        {
            Booking = 1,
            Register,
            Return,
            Table,
            Employee,
            Cancel = 0
        }

        private Dictionary<Dialogs, string> DialogName;
        Dictionary<Dialogs, DialogMetod> DialogMetodDict;

        string DefaultTextMessage;

        public AutoDialog(ChatContext context)
        { 
            _context = context;

            clientState = new Dictionary<Guid, int>();
           
            UserDataDictionary = new Dictionary<Guid, Dictionary<string, string>>();

            DialogName = new Dictionary<Dialogs, string>() {
                { Dialogs.Booking, "Бронювання квитків" },
                { Dialogs.Register, "Онлайн реєстрація" },
                { Dialogs.Employee, "Звязатися з оператором" }
            };

            DialogEmployee dialogEmployee = new DialogEmployee(_context);
            DialogBooking dialogBooking = new DialogBooking();
            DialogRegister dialogRegister = new DialogRegister();

              DialogMetodDict = new Dictionary<Dialogs, DialogMetod>() {
                {Dialogs.Booking,  dialogBooking.DialogBookingMain},
                {Dialogs.Register,  dialogRegister.DialogRegisterMain},
                {Dialogs.Employee,  dialogEmployee.DialogEmployeeMain},

            };

            DefaultTextMessage = ButtonToJson(5, "Оберіть сервіс:", DialogName.Values.ToArray());

        }

        public Message ReplyMessage (Message mes) 
        {


            mes.TextTupe = "text";
            mes.Name = "Bot";

            if (!clientState.ContainsKey(mes.DialogId))
            {

                if (DialogName.ContainsValue(mes.Text))
                {
                    var tupeDialog = DialogName.Where(w => w.Value.Contains(mes.Text)).FirstOrDefault().Key;
                    clientState.Add(mes.DialogId, (int)tupeDialog);
                    mes = DialogMetodDict[tupeDialog](mes, ref clientState);
                }
                  else
                {
                    clientState.Add(mes.DialogId, (int)Dialogs.Cancel);
                    mes.TextTupe = "json";
                    mes.Text = DefaultTextMessage;

                }


            }
            else 
            {
                int tmp = clientState[mes.DialogId];
                if ( tmp == 0)
                {
                    if(DialogName.ContainsValue(mes.Text))
                    {
                        var tupeDialog = DialogName.Where(w => w.Value.Contains(mes.Text)).FirstOrDefault().Key;
                            clientState[mes.DialogId] = (int)tupeDialog; 
                        mes = DialogMetodDict[tupeDialog](mes, ref clientState);
                        
                    }
                    else
                    {
                        mes.TextTupe = "json";
                        mes.Text = clientState[mes.DialogId] == 0 ? DefaultTextMessage : mes.Text;
                    }

                }
                else
                {
                    if(mes.Text.Trim().ToLower().Contains("вийти") )
                    {
                        clientState[mes.DialogId] = (int)Dialogs.Cancel;
                        mes.TextTupe = "json";
                        mes.Text =  DefaultTextMessage;

                    }
                    else
                    {
                        mes = DialogMetodDict[(Dialogs)clientState[mes.DialogId]](mes, ref clientState);
                    }
                       
                }

            }
        
            return mes;

        }

      
        public static string ButtonToJson (int i, string text, string [] buttontext = null)
        {
            var buttons = new
            {
                buttoncount = i,
                text = text,
                textbutton = buttontext

            };
            string json = JsonSerializer.Serialize(buttons);
            return json;
        }




    }
}
