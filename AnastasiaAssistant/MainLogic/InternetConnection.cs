using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
namespace AnastasiaAssistant.MainLogic
{
    class InternetConnection
    {
        public bool TryToConnect()
        {
            IPStatus status = IPStatus.Unknown;
            try
            {
                status = new Ping().Send("8.8.8.8").Status;
            }
            catch { }
            if(status == IPStatus.Success) return true;
            else return false;
        }
         
    }
}
