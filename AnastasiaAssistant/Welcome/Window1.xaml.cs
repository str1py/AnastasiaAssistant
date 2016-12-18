using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Input;

using AnastasiaAssistant.MainLogic.Telgram;
using System.Windows.Forms;
using TLSharp.Core;

namespace AnastasiaAssistant.Welcome
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private static readonly FileSessionStore store = new FileSessionStore();
        public readonly TelegramClient client;



        public int Click = 0;
        public string hash;

        public Window1()
        {
            InitializeComponent();
            MainLogic.InternetConnection ic = new MainLogic.InternetConnection();
            bool connection = ic.TryToConnect();
            if (connection == true)
                client = new TelegramClient(63305, "371ba1c190e97d690122fbf99735ef3f");
        }

        public void ChackFirsOpen()
        {
            if (Properties.Settings.Default.FirstTimeOpen == true)
            {
                MainWindow.Instance.Visibility = System.Windows.Visibility.Hidden;
                this.Show();
                ShowTextBlock("AppearedMEss", WellMes);
                ShowTextBlock("Move", WellMes);
                ShowTextBlock("NameBlock", AskName);
                ShowTextBlock("ButtonOpacity1", ButtonOp);    
            }
            else
            {
                MainWindow.Instance.Visibility = System.Windows.Visibility.Visible;
            }
        }
        private void ShowTextBlock(string Storyboard, StackPanel pnl)
        {
            Storyboard sb = Resources[Storyboard] as Storyboard;
            sb.Begin(pnl);
        }
        public void SecMEss()
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate ()
            {
                TelMess.Text = "Рада познакомиться " + AnastasiaAssistant.Properties.Settings.Default.UserName + "\n"
                           + "Мною можно управлять с помощью Telegram.\nОтправляя сообщения мне в Telegram я буду выполнять их на вашем компьютере."
                           + "\n@AnastasiaAssistant_bot";
                ShowTextBlock("TelMessageApear", Telegram);
            }
            );
        }
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            if (NameBox.Text.Length > 3)
            {
                AnastasiaAssistant.Properties.Settings.Default.UserName = NameBox.Text;
                ShowTextBlock("DisAppearedMEss", WellMes);
                ShowTextBlock("DissApearName", AskName);
                ShowTextBlock("ButtonOpacity0", ButtonOp);
                Next.Visibility = Visibility.Hidden;
                Telegram.Visibility = System.Windows.Visibility.Visible;
                NameBox.IsEnabled = false;
                Thread thread = new Thread(SecMEss);
                thread.Start();
                MainWindow.Instance.CheckAndInfoAdd();
                TelegramEnter.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void Skip_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainWindow.Instance.Show();
        }
        protected async Task WelcomeInit()
        {
            if (String.IsNullOrEmpty(Mobile.Text) == true)
            {
                ErrorMessage.Content = "Phone is null.Please enter your phone";
            }
            else
            {
                // var result = await client.IsPhoneRegisteredAsync(Mobile.Text);
                bool result = true;
                if (result == true)
                {
                    if (Click == 0)
                    {
                        await client.ConnectAsync();
                        hash = await client.SendCodeRequestAsync(Mobile.Text); //отсылаем запрос на создании сессии
                        ShowTextBlock("TelMessageDisapear", Telegram);
                        Telegram.Visibility = System.Windows.Visibility.Hidden;
                        Code.Visibility = System.Windows.Visibility.Visible;
                        ShowTextBlock("CodeTgrm", Code);
                    }
                    else
                    {
                        var code = TelegramCode.Text; // код который придет от Telegram 
                        var user = await client.MakeAuthAsync(Mobile.Text, hash, code); // создаем сессию
                        Properties.Settings.Default.TelegramUserID = user.id.ToString();
                        Properties.Settings.Default.Save();
                        if (user != null)
                        {
                            BotAnswer bot = new BotAnswer();
                            bot.Main();
                            bot.TestMessage();
                            Code.Visibility = System.Windows.Visibility.Hidden;
                            TelegramEnter.Visibility = System.Windows.Visibility.Hidden;
                            MessageSeccuses.Text = "Вход успешно выполнен!";
                            SecMess.Visibility = System.Windows.Visibility.Visible;
                            ShowTextBlock("AuthMess", SecMess);
                        
                        }
                        else
                        {
                            SecMess.Visibility = System.Windows.Visibility.Visible;
                            MessageSeccuses.Text = "Ошибка входа";
                            ShowTextBlock("AuthMess", SecMess);
                        }

                    }
                    Click++;
                }
                else
                {
                    ErrorMessage.Content = "Number is not registered";
                }

            }
        }
        private async void TelegramEnter_Click(object sender, RoutedEventArgs e)
        {
            await WelcomeInit();
        }
        private void ProgramStart_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            MainWindow.Instance.Visibility = Visibility.Visible ;
            Properties.Settings.Default.FirstTimeOpen = false;
            Properties.Settings.Default.Save();
        }
    }
}
