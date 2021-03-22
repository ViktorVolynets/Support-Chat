using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechnicalSupport.Data;
using TechnicalSupport.Models;

namespace TechnicalSupport.AutoChat
{
    public class DialogBooking
    {
      

        private Dictionary<Guid, int> dialogBookingState;
        private Dictionary<Guid, int> dialogNewBookingState;
        private Dictionary<Guid, int> dialogEditBookingState;
       
        string[][] tableflight;
        string[] textArrDialogEditBooking;
        public DialogBooking()
        {
            dialogBookingState = new Dictionary<Guid, int>();
            dialogNewBookingState = new Dictionary<Guid, int>();
            dialogEditBookingState = new Dictionary<Guid, int>();

            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("dialogsettings.json");
            IConfigurationRoot config = builder.Build();

            tableflight = config.GetSection("tableflight").Get<string[][]>();
            textArrDialogEditBooking = config.GetSection("textArrDialogEditBooking").Get<string[]>();
        }

        public Message DialogBookingMain(Message mes, ref Dictionary<Guid, int> clientState)
        {
            mes.TextTupe = "text";
            if (dialogBookingState.ContainsKey(mes.DialogId))
            {
                    switch (dialogBookingState[mes.DialogId])
                    {
                        case 2:
                            mes = DialogBookingNew(mes, ref clientState);
                            return mes;
                        case 3:
                            mes = DialogBookingEdit(mes, ref clientState);
                            return mes;

                    }

                    var textBooking = mes.Text;
                    if (dialogBookingState[mes.DialogId] == 1 && textBooking == "Нове бронювання")
                    {
                        dialogBookingState[mes.DialogId] = 2;

                        mes = DialogBookingNew(mes, ref clientState);
                        return mes;
                    }
                    else
                    {
                            if (dialogBookingState[mes.DialogId] == 1 && textBooking == "Змінити бронювання")
                            {
                                dialogBookingState[mes.DialogId] = 3;

                                mes = DialogBookingEdit(mes, ref clientState);
                                return mes;
                            }
                            else
                            {
                                dialogBookingState[mes.DialogId] = 0;

                            }
                    }
            }
            else
            {

                dialogBookingState.Add(mes.DialogId, 0);


            }


            string ButtoBookingOne = AutoDialog.ButtonToJson(2, "Оберіть дію:", new string[2] { "Нове бронювання", "Змінити бронювання" });

            mes.Text = ButtoBookingOne;
            dialogBookingState[mes.DialogId]++;
            mes.TextTupe = "json";
            return mes;



        }

        public Message DialogBookingNew(Message mes, ref Dictionary<Guid, int> clientState)
        {

            string ButtoBookingOne = AutoDialog.ButtonToJson(4, "Оберіть пункт відправлення:", new string[4] { "Київ", "Харків", "Львів", "Одеса" });

            string ButtoBookingTwo = AutoDialog.ButtonToJson(4, "Оберіть пункт призначення:", new string[4] { "Київ", "Харків", "Львів", "Одеса" });

            string textnumberseat = AutoDialog.ButtonToJson(0, "Введіть номер місця з 1 до 22");
            string flight;
            string numBooking;
            if (AutoDialog.UserDataDictionary.ContainsKey(mes.DialogId) && AutoDialog.UserDataDictionary[mes.DialogId].ContainsKey("Flight")) { flight = AutoDialog.UserDataDictionary[mes.DialogId]["Flight"]; } else { flight = ""; }
            if (AutoDialog.UserDataDictionary.ContainsKey(mes.DialogId) && AutoDialog.UserDataDictionary[mes.DialogId].ContainsKey("NumBooking")) { numBooking = AutoDialog.UserDataDictionary[mes.DialogId]["NumBooking"]; } else { numBooking = ""; }

            string textBookingConfirm = AutoDialog.ButtonToJson(2, $"Підтвердіть бронювання за маршрутом  {flight}", new string[2] { "Yes", "No" });

            string[] textArrDialogNewBooking = { ButtoBookingOne, ButtoBookingTwo, "Оберіть рейс", textnumberseat, textBookingConfirm, "" };



            mes.TextTupe = "json";


            if (dialogNewBookingState.ContainsKey(mes.DialogId))
            {
                var state = dialogNewBookingState[mes.DialogId];

                if (textArrDialogNewBooking.Length > state && state >= 0)
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
                        case 4:
                            valid = ValidationFour(mes);
                            break;
                        case 5:
                            {
                                valid = ValidationFive(mes, ref clientState);
                                textArrDialogNewBooking[5] = valid ? AutoDialog.ButtonToJson(0, $"Дякуємо! Номер бронювання: {AutoDialog.UserDataDictionary[mes.DialogId]["NumBooking"]}") : "";
                                break;
                            }


                    }
                    if (!valid)
                    {

                        mes.Text = textArrDialogNewBooking[--state];
                        return mes;

                    }

                    if (dialogNewBookingState[mes.DialogId] == 2)
                    {
                        List<string> arr = new List<string>();

                        string from = AutoDialog.UserDataDictionary[mes.DialogId]["From"];
                        string to = AutoDialog.UserDataDictionary[mes.DialogId]["To"];

                        for (int i = 0; i < tableflight.Length; i++)
                        {

                            if (tableflight[i][0] == from && tableflight[i][1] == to)
                            {

                                arr.Add(tableflight[i][0] + "-" + tableflight[i][1] + " " + tableflight[i][2]);

                            }

                        }

                        if (arr.Count() > 0)
                        {

                            mes.Text = AutoDialog.ButtonToJson(arr.Count(), "Оберіть маршрут:", arr.ToArray());
                            dialogNewBookingState[mes.DialogId]++;
                        }
                        else
                        {
                            mes.TextTupe = "text";
                            mes.Text = "Маршрутів не знайдено! Змініть пункт призначення.";
                            dialogNewBookingState[mes.DialogId]--;
                        }

                    }
                    else
                    {
                        mes.Text = textArrDialogNewBooking[state];
                        dialogNewBookingState[mes.DialogId]++;


                    }





                    if (textArrDialogNewBooking.Length <= dialogNewBookingState[mes.DialogId])
                    {
                        clientState[mes.DialogId] = 0;
                        dialogBookingState[mes.DialogId] = 0;
                        dialogNewBookingState[mes.DialogId] = 0;

                    }
                    return mes;

                }

            }
            else
            {

                dialogNewBookingState.Add(mes.DialogId, 0);
                mes.Text = textArrDialogNewBooking[0];
                dialogNewBookingState[mes.DialogId]++;

            }


            return mes;


            bool ValidationOne(Message mes)
            {
                if (dialogNewBookingState[mes.DialogId] == 1 && tableflight.Any(x => x.Contains(mes.Text)))
                {

                    if (!AutoDialog.UserDataDictionary.ContainsKey(mes.DialogId))
                    {
                        AutoDialog.UserDataDictionary.Add(mes.DialogId, new Dictionary<string, string>() { { "From", mes.Text } });

                    }
                    else
                    {
                        if (AutoDialog.UserDataDictionary[mes.DialogId].ContainsKey("From"))
                        {
                            AutoDialog.UserDataDictionary[mes.DialogId]["From"] = mes.Text;

                        }
                        else
                        {
                            AutoDialog.UserDataDictionary[mes.DialogId].Add("From", mes.Text);
                        }

                    }
                    return true;

                }

                return false;
            }

            bool ValidationTwo(Message mes)
            {
                if (dialogNewBookingState[mes.DialogId] == 2 && tableflight.Any(x => x.Contains(mes.Text)))
                {

                    if (!AutoDialog.UserDataDictionary.ContainsKey(mes.DialogId))
                    {
                        AutoDialog.UserDataDictionary.Add(mes.DialogId, new Dictionary<string, string>() { { "To", mes.Text } });

                    }
                    else
                    {
                        if (AutoDialog.UserDataDictionary[mes.DialogId].ContainsKey("To"))
                        {
                            AutoDialog.UserDataDictionary[mes.DialogId]["To"] = mes.Text;

                        }
                        else
                        {
                            AutoDialog.UserDataDictionary[mes.DialogId].Add("To", mes.Text);
                        }

                    }

                    return true;

                }
                return false;
            }

            bool ValidationThree(Message mes)
            {
                List<string> arr = new List<string>();

                for (int i = 0; i < tableflight.Length; i++)
                {
                    arr.Add(tableflight[i][0] + "-" + tableflight[i][1] + " " + tableflight[i][2]);
                }


                if (dialogNewBookingState[mes.DialogId] == 3 && arr.Contains(mes.Text))
                {
                    if (!AutoDialog.UserDataDictionary.ContainsKey(mes.DialogId))
                    {
                        AutoDialog.UserDataDictionary.Add(mes.DialogId, new Dictionary<string, string>() { { "Flight", mes.Text } });

                    }
                    else
                    {
                        if (AutoDialog.UserDataDictionary[mes.DialogId].ContainsKey("Flight"))
                        {
                            AutoDialog.UserDataDictionary[mes.DialogId]["Flight"] = mes.Text;

                        }
                        else
                        {
                            AutoDialog.UserDataDictionary[mes.DialogId].Add("Flight", mes.Text);
                        }

                    }
                    return true;
                }

                return false;
            }

            bool ValidationFour(Message mes)
            {
                if (dialogNewBookingState[mes.DialogId] == 4)
                {

                    int numberseat;

                    if (int.TryParse(mes.Text, out numberseat) && numberseat <= 22 && numberseat > 0)
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

            bool ValidationFive(Message mes,ref Dictionary<Guid, int> clientState)
            {
                if (dialogNewBookingState[mes.DialogId] == 5)
                {


                    if (mes.Text == "Yes")
                    {
                        char[] letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
                        string numbooking = "";
                        Random rand = new Random();

                        for (int j = 0; j <= 6; j++)
                        {

                            if (j == 0 || j == 1)
                            {
                                int num = rand.Next(0, letters.Length - 1);
                                numbooking += letters[num];
                            }
                            else
                            {
                                int num = rand.Next(0, 9);
                                numbooking += num;

                            }

                        }

                        if (!AutoDialog.UserDataDictionary.ContainsKey(mes.DialogId))
                        {
                            AutoDialog.UserDataDictionary.Add(mes.DialogId, new Dictionary<string, string>() { { "NumBooking", numbooking } });
                            return true;
                        }
                        else
                        {

                            if (AutoDialog.UserDataDictionary[mes.DialogId].ContainsKey("NumBooking"))
                            {
                                AutoDialog.UserDataDictionary[mes.DialogId]["NumBooking"] = numbooking;
                            }
                            else
                            {
                                AutoDialog.UserDataDictionary[mes.DialogId].Add("NumBooking", numbooking);
                            }
                        }


                        return true;


                    }
                    else
                    {

                        clientState[mes.DialogId] = 0;
                        dialogBookingState[mes.DialogId] = 0;
                        dialogNewBookingState[mes.DialogId] = 0;

                        return false;

                    }
                }
                return false;
            }

        }

        public Message DialogBookingEdit(Message mes, ref Dictionary<Guid, int> clientState)
        {

            if (dialogEditBookingState.ContainsKey(mes.DialogId))
            {
                var state = dialogEditBookingState[mes.DialogId];

                if (textArrDialogEditBooking.Length > state && state >= 0)
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

                        mes.Text = textArrDialogEditBooking[--state];
                        return mes;

                    }

                    if (dialogEditBookingState[mes.DialogId] == 1)
                    {
                        var booking = AutoDialog.UserDataDictionary.Values.Where(v => v.ContainsValue(mes.Text)).FirstOrDefault();

                        if (!AutoDialog.UserDataDictionary.ContainsKey(mes.DialogId))
                        {
                            AutoDialog.UserDataDictionary.Add(mes.DialogId, booking);

                        }
                        else
                        {

                            AutoDialog.UserDataDictionary[mes.DialogId] = booking;


                        }

                        string textBooking = AutoDialog.ButtonToJson(2, $"Змінити бронювання за маршрутом  {booking["Flight"]} Місце :{booking["NumSeat"]} ", new string[2] { "Відмінити", "Змінити місце" });
                        mes.TextTupe = "json";
                        textArrDialogEditBooking[state] = textBooking;
                    }
                    if (dialogEditBookingState[mes.DialogId] == 2)
                    {

                        if (mes.Text == "Відмінити")
                        {

                            AutoDialog.UserDataDictionary[mes.DialogId].Remove("NumBooking");
                            mes.Text = "Бронювання відмінено!";
                            mes.TextTupe = "text";
                            clientState[mes.DialogId] = 0;
                            dialogBookingState[mes.DialogId] = 0;
                            dialogEditBookingState[mes.DialogId] = 0;

                            return mes;

                        }


                    }

                    mes.Text = textArrDialogEditBooking[state];
                    dialogEditBookingState[mes.DialogId]++;


                    if (textArrDialogEditBooking.Length <= dialogEditBookingState[mes.DialogId])
                    {
                        clientState[mes.DialogId] = 0;
                        dialogBookingState[mes.DialogId] = 0;
                        dialogEditBookingState[mes.DialogId] = 0;

                    }
                    return mes;

                }
            }
            else
            {
                dialogEditBookingState.Add(mes.DialogId, 0);
                mes.Text = textArrDialogEditBooking[0];
                dialogEditBookingState[mes.DialogId]++;

            }


            return mes;

            bool ValidationOne(Message mes)
            {

                if (dialogEditBookingState[mes.DialogId] == 1)
                {
                    var numberBooking = mes.Text;
                    if (AutoDialog.UserDataDictionary.Values.Any(v => v.ContainsValue(numberBooking)))
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
            bool ValidationTwo(Message mes)
            {

                if (dialogEditBookingState[mes.DialogId] == 2 && (mes.Text == "Відмінити" || mes.Text == "Змінити місце"))
                {

                    return true;


                }

                return false;

            }
            bool ValidationThree(Message mes)
            {
                if (dialogEditBookingState[mes.DialogId] == 3)
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



        }




    }
}
