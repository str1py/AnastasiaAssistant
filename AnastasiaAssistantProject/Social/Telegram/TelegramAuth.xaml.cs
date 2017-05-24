using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TLSharp.Core;
using System.Windows.Threading;
using System.Threading;
namespace AnastasiaAssistantProject.Social.Telgram
{
    /// <summary>
    /// Логика взаимодействия для TelegramAuth.xaml
    /// </summary>
    class TelegramAuth
    {
        private static readonly FileSessionStore store = new FileSessionStore();
        private  TelegramClient client;
        public int Click = 0;
        string hash;
        string CurrectPhone;
        public string ErrorMessage { get; set; }



        public async Task PhoneCheck(string Phone)
        {
            if(MainAppLogic.MainVars.InternetConnection == false)
            {
                ErrorMessage = "No Internet Connection";
            }
            else
            {
                client = new TelegramClient(63305, "371ba1c190e97d690122fbf99735ef3f");
                if (String.IsNullOrEmpty(Phone) == true)
                    ErrorMessage = "Phone is null.Please enter your phone";
                else
                {
                    try
                    {
                        await client.ConnectAsync();
                        hash = await client.SendCodeRequestAsync(Phone);
                        MainAppLogic.MainVars.isTelegramPhoneCorrect = true;
                        CurrectPhone = Phone;
                    }
                    catch(Exception ex)
                    { ErrorMessage = "Ошибка входа"; MessageBox.Show(ex.ToString()); }
                }
        
            }
        }

        public async Task CodeCheck(string Code)
        {
            var code = Code;
            try
            {
               var user = await client.MakeAuthAsync(CurrectPhone, hash, code);

                if (user != null)
                {
                    MainAppLogic.MainVars.isTelegramLogicSeccused = true;
                    Properties.Settings.Default.TelegramUserID = user.id.ToString();
                    Properties.Settings.Default.TelegramUserName = user.first_name;
                    Properties.Settings.Default.TelegramUserSurName = user.last_name;
                }
            }
            catch
            {
                ErrorMessage = "The code doens`t match. Try again.";

            }


        }



        private void ExitClick_Click(object sender, RoutedEventArgs e)
        {
           // MainVars.TelegramUserID = 0;
            //this.Close();

        }

    }
}
