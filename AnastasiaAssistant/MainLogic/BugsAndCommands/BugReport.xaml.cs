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

namespace AnastasiaAssistant.MainLogic.BugsAndCommands
{
    /// <summary>
    /// Логика взаимодействия для BugReport.xaml
    /// </summary>
    public partial class BugReport : Window
    {
        public BugReport()
        {
            InitializeComponent();
        }

        private void SendBug_Click(object sender, RoutedEventArgs e)
        {
            CoomandBugsMethods.SendBugReport(BugReason.SelectedIndex, BugDiscr.Text);
            this.Close();
        }
    }
}
