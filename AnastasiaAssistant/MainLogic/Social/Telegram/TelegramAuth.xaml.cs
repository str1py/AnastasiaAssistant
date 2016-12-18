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
namespace AnastasiaAssistant.MainLogic.Telgram
{
    /// <summary>
    /// Логика взаимодействия для TelegramAuth.xaml
    /// </summary>
    public partial class TelegramAuth : Window
    {
        private static readonly FileSessionStore store = new FileSessionStore();
        private readonly TelegramClient client;
        public int Click = 0;
        string hash;

        public TelegramAuth()
        {
            InitializeComponent();
            InternetConnection ic = new InternetConnection();
            bool connection = ic.TryToConnect();
            if(connection==true)
                client = new TelegramClient(63305, "371ba1c190e97d690122fbf99735ef3f"); 
        }

        protected async Task Init()
        {
            if (String.IsNullOrEmpty(TelegramPhone.Text) == true)
            {
                ErrorMess.Content = "Phone is null.Please enter your phone";
            }
            else
            {
                // var result = await client.IsPhoneRegisteredAsync(Mobile.Text);
                bool result = true;
                if (result == true)
                {
                    if (Click == 0)
                    {
                        PhonePanel.Visibility = Visibility.Hidden; CodePanel.Visibility = Visibility.Visible;
                        await client.ConnectAsync();
                        hash = await client.SendCodeRequestAsync(TelegramPhone.Text);
                    }
                    else
                    {
                        var code = TelegramCode.Text; 
                        var user = await client.MakeAuthAsync(TelegramPhone.Text, hash, code);       
                        if (user != null)
                        {
                            MainWindow.Instance.TeleAuth.Visibility = System.Windows.Visibility.Hidden;
                            MainWindow.Instance.TeleDeAuth.Visibility = System.Windows.Visibility.Visible;
                            MainWindow.Instance.TeleDeAuth.Content = user.first_name +" "+ user.last_name + "\nLogout";
                            Uri imagePlayUri = (new Uri(@"pack://application:,,,/AnastasiaRes/Social/t_logo.png", UriKind.Absolute));
                            MainWindow.Instance.TelegramPicture.Source = new BitmapImage(imagePlayUri);
                            Properties.Settings.Default.TelegramUserID = user.id.ToString();
                            Properties.Settings.Default.TelegramUserName = user.first_name;
                            Properties.Settings.Default.TelegramUserSurName = user.last_name;
                            Properties.Settings.Default.Save();
                            BotAnswer bot = new BotAnswer();
                            bot.Main();
                            bot.TestMessage();
                            this.Close();
                 
                        }
                        else
                        {
                            ErrorMess.Content = "Ошибка входа";      
                        }
                    }
                    Click++;
                }
                else
                {
                    ErrorMess.Content = "Number is not registered";
                }

            }
        }

        private async void PhoneCheck_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Init();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ExitClick_Click(object sender, RoutedEventArgs e)
        {
            MainVars.TelegramUserID = 0;
            this.Close();

        }
        private void TelegramBorder_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
