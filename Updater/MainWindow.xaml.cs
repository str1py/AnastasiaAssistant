using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Updater
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        
            UpdateNow();
        }

        public async void UpdateNow()
        {
            this.Show();
            bool oldanastasia = true;
            bool newanastasia = false;
            Changes.Content = "Идет проверка целостности файлов...";
            await Task.Delay(3000);
            string path = Directory.GetCurrentDirectory();

            string[] allFoundFiles = Directory.GetFiles(path, "AnastasiaAssistantProject.exe");
            foreach (string file in allFoundFiles)
            {
                 oldanastasia = true;
            }

            string[] allFoundFiles1 = Directory.GetFiles(path, "AnastasiaAssistantProject.update");
            foreach (string file in allFoundFiles)
            {
                 newanastasia = true;
            }

          
            if (oldanastasia == true && newanastasia == true)
            {
                Changes.Content = "Все файлы на месте. Идет применение настроек...";
                await Task.Delay(3000);
                try
                {
                    FileInfo fi1 = new FileInfo(path + @"\AnastasiaAssistantProject.exe");
                    fi1.Delete();
                }
                catch (Exception ex)
                {
                    Changes.Content = ex.ToString();
                }
                await Task.Delay(3000);
                try
                {
                    System.IO.File.Move(path + @"\AnastasiaAssistantProject.update", path + @"\AnastasiaAssistantProject.exe");
                    Changes.Content = "Все готово к работе :)";
                    await Task.Delay(3000);
                    System.Diagnostics.Process.Start(path + @"/AnastasiaAssistantProject.exe");
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    Changes.Content = ex.ToString();
                }

            }
            else
            {
                Changes.Content = "Что - то не так";
            }
        }
    }

}
