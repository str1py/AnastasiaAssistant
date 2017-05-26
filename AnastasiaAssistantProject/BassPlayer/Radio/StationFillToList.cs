using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AnastasiaAssistantProject.BassPlayer.Radio
{
    class StationFillToList
    {
        public void RecordStationsFill()
        {
            string path = System.IO.Directory.GetCurrentDirectory();
            XDocument xDoc = XDocument.Load(path + @"/Data/RecordStations.xml");

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
            string path = System.IO.Directory.GetCurrentDirectory();
            XDocument xDoc1 = XDocument.Load(path + @"/Data/MoscowRadioStations.xml");

            int station = 6;
            string st;
            for (int i = 1; i <= station; i++)
            {
                st = xDoc1.Root.Element("MRS" + i).Element("title").Value;
                MainWindow.Instance.AddStations(st);
            }
        }
        public void BBCStationsFill()
        {
            string path = System.IO.Directory.GetCurrentDirectory();
            XDocument xDoc2 = XDocument.Load(path + @"/Data/BBCRadioStations.xml");

            int station = 13;
            string st;
            for (int i = 1; i <= station; i++)
            {
                st = xDoc2.Root.Element("BBCRadio" + i).Element("title").Value;
                MainWindow.Instance.AddStations(st);
            }
        }
        public void LastFmStationsFill()
        {

        }
    }
}
