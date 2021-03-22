using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalSupport.Data;
using TechnicalSupport.Models;

namespace TechnicalSupport.AutoChat
{
    public class DialogRegister
    {
       

        private Dictionary<Guid, int> dialogRegState;
       
        string[] textArrDialogRegister;

        public DialogRegister()
        {    
            dialogRegState = new Dictionary<Guid, int>();

            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("dialogsettings.json");
            IConfigurationRoot config = builder.Build();

            textArrDialogRegister = config.GetSection("textArrDialogRegister").Get<string[]>();

        }

        public Message DialogRegisterMain (Message mes, ref Dictionary<Guid, int> clientState)
        {

            if (dialogRegState.ContainsKey(mes.DialogId))
            {

                var state = dialogRegState[mes.DialogId];

                if (textArrDialogRegister.Length > state && state >= 0)
                {
                    bool valid = false;
                    switch (state)
                    {
                        case 0:
                            valid = true;
                            break;
                        case 1:
                            valid = ValidationOne(mes);
                            break;
                        case 2:
                            valid = ValidationTwo(mes);
                            break;
                        case 3:
                            valid = ValidationThree(mes);
                            break;

                    }
                    if (!valid)
                    {
                        mes.Text = "Не вірно введені дані " + textArrDialogRegister[--state];
 
                        return mes;

                    }
                    mes.Text = textArrDialogRegister[state];
                    dialogRegState[mes.DialogId]++;

                    if (textArrDialogRegister.Length <= dialogRegState[mes.DialogId])
                    {
                        clientState[mes.DialogId] = 0;
                        dialogRegState[mes.DialogId] = 0;

                    }
                    return mes;

                }

            }
            else
            {
                dialogRegState.Add(mes.DialogId, 0);
            }

            mes.Text = textArrDialogRegister[dialogRegState[mes.DialogId]];
            dialogRegState[mes.DialogId]++;

            return mes;


            bool ValidationOne(Message mes)
            {
                if (dialogRegState[mes.DialogId] == 1)
                {
                    var numberBooking = mes.Text;
                    if (AutoDialog.UserDataDictionary.Values.Any(v => v.ContainsValue(numberBooking)))
                    {
                        if (!AutoDialog.UserDataDictionary.ContainsKey(mes.DialogId))
                        {
                            AutoDialog.UserDataDictionary.Add(mes.DialogId, new Dictionary<string, string>() { { "NumBooking", mes.Text } });
                            return true;
                        }
                        else
                        {
                            if (AutoDialog.UserDataDictionary[mes.DialogId].ContainsKey("NumBooking"))
                            {
                                AutoDialog.UserDataDictionary[mes.DialogId]["NumBooking"] = mes.Text;
                            }
                            else
                            {
                                AutoDialog.UserDataDictionary[mes.DialogId].Add("NumBooking", mes.Text);

                            }
                            return true;
                        }

                    }
                    else
                    {
                        return false;

                    }

                }

                return false;

            }

            bool ValidationTwo(Message mes)
            {
                if (dialogRegState[mes.DialogId] == 2)
                {

                    int numberseat;

                    if (int.TryParse(mes.Text, out numberseat) && numberseat <= 44 && numberseat > 0)
                    {
                        if (AutoDialog.UserDataDictionary[mes.DialogId].ContainsKey("NumSeat"))
                        {
                            AutoDialog.UserDataDictionary[mes.DialogId]["NumSeat"] = mes.Text;
                        }
                        else
                        {
                            AutoDialog.UserDataDictionary[mes.DialogId].Add("NumSeat", mes.Text);
                        }


                        return true;
                    }
                    else
                    {
                        return false;

                    }

                }

                return false;

            }

            bool ValidationThree(Message mes)
            {
                if (dialogRegState[mes.DialogId] == 3)
                {
                    if (AutoDialog.UserDataDictionary[mes.DialogId]["NumBooking"] != null && AutoDialog.UserDataDictionary[mes.DialogId]["NumSeat"] != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                return false;

            }

        }

    }

}
