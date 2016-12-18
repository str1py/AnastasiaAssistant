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
    class GetFullMusicInfo : BassNetHelper
    {
        public static void GetTrackInfo()
        {
            BassNetData.SongDataClear();
            var audioFile = TagLib.File.Create(Files[CurrentTrackNumber]);
            if (audioFile.Tag.Pictures.Length != 0)
            {
                TagLib.IPicture pic = audioFile.Tag.Pictures[0];
                MemoryStream ms = new MemoryStream(pic.Data.Data);
                ms.Seek(0, SeekOrigin.Begin);
                BitmapImage bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                BassNetData.songPicture = bitmap;
            }
            else
            {
                Uri imagePlayUri = (new Uri(@"pack://application:,,,/AnastasiaRes/Player/Music/MusicDefault.png", UriKind.Absolute));
                BassNetData.songPicture = new BitmapImage(imagePlayUri);
            }
            if (String.IsNullOrEmpty(audioFile.Tag.FirstPerformer) == false)
            {
                BassNetData.artist = audioFile.Tag.FirstPerformer;
                BassNetData.songName = audioFile.Tag.Title;
                MainVars.SongName = "Song: " + audioFile.Tag.FirstPerformer + " - " + audioFile.Tag.Title;
                MainVars.AnastasiaAnswer = MainLogic.MainVars.SongName;
            }
            else
            {
                string WithOutMP3 = Path.GetFileName(Files[CurrentTrackNumber]);
                BassNetData.artist = WithOutMP3.Substring(0, WithOutMP3.Length - 4);
                MainVars.SongName = "Song:" + WithOutMP3.Substring(0, WithOutMP3.Length - 4);
                MainVars.AnastasiaAnswer = MainLogic.MainVars.SongName;
            }
            BassNetData.songTime = TimeSpan.FromSeconds(GetTimeOfStream(Stream)).ToString();
            BassNetData.sliderMaximum = GetTimeOfStream(Stream);
        }
        public static void PopularMusicGanre(string Genre)
        {
            string genre = Genre;
            if (genre == "house") { UserInfo.Default.house++; }
            if (genre == "dance") { UserInfo.Default.dance++; }
            if (genre == "trap") { UserInfo.Default.trap++; }
            if (genre == "electronic") { UserInfo.Default.electronic++; }
            if (genre == "blues") { UserInfo.Default.blues++; }
            if (genre == "funky") { UserInfo.Default.funky++; }
            if (genre == "alt. rock" || genre == "alt.rock") { UserInfo.Default.altrock++; }
            if (genre == "dubstep") { UserInfo.Default.dubstep++; }
            if (genre == "hip-hop") { UserInfo.Default.hiphop++; }
            if (genre == "moombahton") { UserInfo.Default.moombahton++; }
            if (genre == "trance") { UserInfo.Default.trance++; }
            if (genre == "pop") { UserInfo.Default.pop++; }
            if (genre == "future bass") { UserInfo.Default.futurebass++; }
            if (genre == "deep house") { UserInfo.Default.deephouse++; ; }
            if (genre == "drum & bass") { UserInfo.Default.drumbass++; }
            if (genre == "rock") { UserInfo.Default.rock++; }
            if (genre == "instrumental") { UserInfo.Default.instrumental++; }
            if (genre == "electro house") { UserInfo.Default.electrohouse++; }
            if (genre == "alternative") { UserInfo.Default.alternative++; }
            if (genre == "glitch hop") { UserInfo.Default.glitchhop++; }
            UserInfo.Default.Save();
        }
    }

}
