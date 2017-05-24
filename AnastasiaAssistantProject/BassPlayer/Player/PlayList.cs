using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AnastasiaAssistantProject.BassPlayer
{
    public class PlayList
    {

        public BitmapImage SongPict { get; set; }
        public string ArtName { get; set; }
        public string SongName { get; set; }

        public override string ToString()
        {
            return this.ArtName + ", " + this.SongName;
        }
    }
    public class PlayListMethods:BassNetHelper
    {
        public void PlayListFill()
        {
            playlist.Clear();
            int count = Files.Count();
            for (int i = 0; i < count; i++)
            {
                var audioFile = TagLib.File.Create(Files[i]);
                if (audioFile.Tag.Pictures.Length != 0)
                {
                    if (String.IsNullOrEmpty(audioFile.Tag.FirstPerformer) == false)
                    {
                        TagLib.IPicture pic = audioFile.Tag.Pictures[0];
                        MemoryStream ms = new MemoryStream(pic.Data.Data);
                        ms.Seek(0, SeekOrigin.Begin);
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = ms;
                        bitmap.EndInit();
                        playlist.Add(new PlayList()
                        {
                            ArtName = audioFile.Tag.FirstPerformer,
                            SongName = audioFile.Tag.Title,
                            SongPict = bitmap
                        });
                    }
                    else
                    {
                        TagLib.IPicture pic = audioFile.Tag.Pictures[0];
                        MemoryStream ms = new MemoryStream(pic.Data.Data);
                        ms.Seek(0, SeekOrigin.Begin);
                        BitmapImage bitmap = new BitmapImage();
                        bitmap.BeginInit();
                        bitmap.StreamSource = ms;
                        bitmap.EndInit();
                        string WithOutMP3 = Path.GetFileName(Files[i]);
                        playlist.Add(new PlayList()
                        {
                            ArtName = WithOutMP3,
                            SongPict = bitmap
                        });
                    }
                }
                else
                {
                    if (String.IsNullOrEmpty(audioFile.Tag.FirstPerformer) == false)
                    {
                        System.Windows.Media.Imaging.BitmapImage b = new BitmapImage(new Uri("pack://application:,,,/AnastasiaResources/Player/MusicDefault.png"));
                        playlist.Add(new PlayList()
                        {
                            ArtName = audioFile.Tag.FirstPerformer,
                            SongName = audioFile.Tag.Title,
                            SongPict = b
                        });
                    }
                    else
                    {
                        System.Windows.Media.Imaging.BitmapImage b = new BitmapImage(new Uri("pack://application:,,,/AnastasiaResources/Player/MusicDefault.png"));
                        string WithOutMP3 = Path.GetFileName(Files[i]);
                        playlist.Add(new PlayList()
                        {
                            ArtName = WithOutMP3,
                            SongPict = b
                        });
                    }
                }
            }
            MainWindow.Instance.PlayList.ItemsSource = playlist;
        }
        public void ClearPlayList()
        {
            playlist.Clear();
            MainWindow.Instance.PlayList.ItemsSource = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

    }
 
  
}
