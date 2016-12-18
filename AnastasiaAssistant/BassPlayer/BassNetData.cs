using AnastasiaAssistant.MainLogic;
using AnastasiaAssistant.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace AnastasiaAssistant.BassPlayer
{
    class BassNetData
    {
        public static string artist { get; set; }
        public static string songName { get; set; }
        public static BitmapImage songPicture { get; set; }
        public static string songTime { get; set; }
        public static double sliderMaximum { get; set; }
        public static void SongDataClear()
        {
            artist = null;
            songName = null;
            songPicture = null;
            songTime = null;
            sliderMaximum = 0;
        }   

    }

}
