using System;
using MySql.Data.MySqlClient;
using System.Windows;
using System.Data;

namespace AnastasiaAssistant.MainLogic.BugsAndCommands
{
    /// <summary>
    /// Логика взаимодействия для CommandRequest.xaml
    /// </summary>
    public partial class CommandRequest : Window
    {
        public CommandRequest()
        {
            InitializeComponent();
        }

        private void SendBug_Click(object sender, RoutedEventArgs e)
        {
            
            if(String.IsNullOrEmpty(NewCommand.Text)==false)
            {
                CoomandBugsMethods.SendCommandRequest(NewCommand.Text,Discription.Text);
                this.Close();
            }

        }
    }
}
