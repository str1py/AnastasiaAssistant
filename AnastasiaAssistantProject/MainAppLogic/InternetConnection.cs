using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace AnastasiaAssistantProject.MainAppLogic
{
    class InternetConnection
    {
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        public bool TryToConnect()
        {
            IPStatus status = IPStatus.Unknown;
            try
            {
                status = new Ping().Send("8.8.8.8").Status;
            }
            catch { }
            if (status == IPStatus.Success) return true;
            else return false;
        }


        public void TimerInternetCheck()
        {
            IPStatus status = IPStatus.Unknown;
            try
            {
                status = new Ping().Send("8.8.8.8").Status;
            }
            catch { }
            if (status == IPStatus.Success) MainVars.InternetConnection=true;
            else MainVars.InternetConnection= false;
        }
        public void InternetConnectionTimerStart()
        {
            dispatcherTimer.Tick += new EventHandler(timerTick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 2, 0, 0);
            dispatcherTimer.Start();
        }
        private void timerStop()
        {
            dispatcherTimer.Stop();
            dispatcherTimer.IsEnabled = false;
        }
        public void timerTick(object sender, EventArgs e)
        {
            TimerInternetCheck();
        }


    }
}
