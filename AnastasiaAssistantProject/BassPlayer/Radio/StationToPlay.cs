using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Un4seen.Bass.AddOn.Tags;

namespace AnastasiaAssistantProject.BassPlayer.Radio
{
    class StationToPlay
    {
        System.Windows.Threading.DispatcherTimer UpdateStationInfo = new System.Windows.Threading.DispatcherTimer();

        public static string StationName;
        public static string TrackName;
        public static string TrackArtist;
        public static string TrackGenre;
        public static string ChannelInfo;
        public static string URL;

        public void Getindex(int stationindex, int stationgroup)
        {
            if (stationgroup == 0)
            {
                string path = System.IO.Directory.GetCurrentDirectory();
                XDocument xDoc = XDocument.Load(path + @"/Data/RecordStations.xml");
                URL = xDoc.Root.Element("RecordStation" + (stationindex + 1)).Element("link").Value;
                StationName = xDoc.Root.Element("RecordStation" + (stationindex + 1)).Element("title").Value;
                timerStart();
            }
            if (stationgroup == 1)
            {
                string path = System.IO.Directory.GetCurrentDirectory();
                XDocument xDoc = XDocument.Load(path + @"/Data/MoscowRadioStations.xml");
                URL = xDoc.Root.Element("MRS" + (stationindex + 1)).Element("link").Value;
                StationName = xDoc.Root.Element("MRS" + (stationindex + 1)).Element("title").Value;
                timerStart();
            }
            if(stationgroup==2)
            {
                string path = System.IO.Directory.GetCurrentDirectory();
                XDocument xDoc = XDocument.Load(path + @"/Data/BBCRadioStations.xml");
                URL = xDoc.Root.Element("BBCRadio" + (stationindex + 1)).Element("link").Value;
                StationName = xDoc.Root.Element("BBCRadio" + (stationindex + 1)).Element("title").Value;
                timerStart();
            }
        }



        public void GetStationInfo()
        {
            TAG_INFO tagInfo = new TAG_INFO(URL);
            if (BassTags.BASS_TAG_GetFromURL(BassNetHelper.Stream, tagInfo))
            {
                TrackArtist = tagInfo.artist;
                TrackName = tagInfo.title;
                TrackGenre = tagInfo.genre;

                if (tagInfo.bpm == "") { ChannelInfo = "Bitrate : " + tagInfo.bitrate; }
                else { ChannelInfo = "Bitrate : " + tagInfo.bitrate + "kBit/sec   Bpm : " + tagInfo.bpm; }
            }
            else{ TrackArtist = "No Data"; TrackName = "No Data"; }

        }

            private void timerStart()
            {
                UpdateStationInfo.Tick += new EventHandler(timerTick);
                UpdateStationInfo.Interval = new TimeSpan(0, 0, 0, 30, 0);
                UpdateStationInfo.Start();
            }
            public void timerStop()
            {
                UpdateStationInfo.Stop();
            }
            public void timerTick(object sender, EventArgs e)
            {
                GetStationInfo();
                MainWindow.Instance.SetStationInfo();
                MainAppLogic.MainVars.SongName = TrackArtist = "-" + TrackName;
            }
    }
}
