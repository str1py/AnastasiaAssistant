using AnastasiaAssistantProject.MainAppLogic;
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

namespace AnastasiaAssistantProject
{
    /// <summary>
    /// Логика взаимодействия для StartUpWindow.xaml
    /// </summary>
    public partial class StartUpWindow : Window
    {
        public StartUpWindow()
        {
            InitializeComponent();
            MainInitialization();
        }
        public async void MainInitialization()
        {
            MainWindow main = new MainWindow();
  
            MainVars mv = new MainVars();
            InitLabel.Content = "Загрузка...";
            await Task.Delay(1000);

            InitLabel.Content = "Инициализация команд...";
            await Task.Delay(1000);
            SearchAnswer.InitCommandList();

            InitLabel.Content = "Проверка интернет соединения...";
            await Task.Delay(1000);
            InternetConnection ic = new InternetConnection();
            MainVars.InternetConnection = ic.TryToConnect();
            ic.InternetConnectionTimerStart();

            InitLabel.Content = "Настройка Telegram...";
            main.InitTelegram();
            await Task.Delay(1000);

            InitLabel.Content = "Запуск...";
            await Task.Delay(1000);
            main.InitHelper();
            main.Show();
            this.Close();

        }
    }
}
