using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;

namespace AnastasiaAssistant.BassPlayer.Radio
{
    class StationListFill
    {
        public void RecordStationsFill()
        {
            string path = System.IO.Directory.GetCurrentDirectory();
            XDocument xDoc = XDocument.Load(path + @"/RecordStations.xml");

            int station = 18;
            string st;
            for (int i = 1; i <= station; i++) 
            {
                st = xDoc.Root.Element("RecordStation" + i).Element("title").Value;
                MainWindow.Instance.AddStations(st);
            }



        }
        public void MoscowStationsFill()
        {

        }
    }
}
