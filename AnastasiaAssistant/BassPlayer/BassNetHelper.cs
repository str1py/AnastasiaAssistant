using System;
using System.Collections.Generic;
using System.Linq;
using Un4seen.Bass;
using System.Windows.Forms;
using System.IO;
using System.Windows.Media.Imaging;
using Un4seen.Bass.AddOn.Tags;
using System.Drawing;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using AnastasiaAssistant.Properties;
using System.Resources;
using System.Reflection;
using AnastasiaAssistant.MainLogic;
namespace AnastasiaAssistant.BassPlayer
{

    public class  BassNetHelper
    {
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer songNameChange = new System.Windows.Threading.DispatcherTimer();
        protected ObservableCollection<PlayList> playlist = new ObservableCollection<PlayList>();

        private static int HZ = 44100;
        private static bool InitDefaultDevice;
        public static int Stream;
        public static float Volume = 100;
        protected static List<string> Files = new List<string>();
        private static int alltrackscount;
        protected static int CurrentTrackNumber = 0;
        private static bool isStopped = true;
        private static bool EndPlayList;
 

        #region MainMethods
        private static bool InitBass(int hz)
        {
            if (!InitDefaultDevice)
                InitDefaultDevice = Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
            return InitDefaultDevice;
        }
        public static void SetStreamVolume(int stream, float vol)
        {
            Volume = vol;
            Bass.BASS_ChannelSetAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, Volume / 100F);
        }
        protected static int GetTimeOfStream(int stream)
        {
            long TimeBytes = Bass.BASS_ChannelGetLength(stream);
            double Time = Bass.BASS_ChannelBytes2Seconds(stream, TimeBytes);
            return (int)Time;
        }
        private static int GetStreamPos(int steam)
        {
            long pos = Bass.BASS_ChannelGetPosition(steam);
            int posSec = (int)Bass.BASS_ChannelBytes2Seconds(steam, pos);
            return posSec;
        }
        public static void SetPosOfScroll(int stream, double pos)
        {
            Bass.BASS_ChannelSetPosition(stream, pos);
        }
        #endregion

        #region Методы кнопок
        protected static void Play(string track, float vol)
        {
            if (Bass.BASS_ChannelIsActive(Stream) != BASSActive.BASS_ACTIVE_PAUSED)
            {
                Stop();
                if (InitBass(HZ))
                {
                    Stream = Bass.BASS_StreamCreateFile(track, 0, 0, BASSFlag.BASS_DEFAULT);
                    if (Stream != 0)
                    {
                        Volume = vol;
                        Bass.BASS_ChannelSetAttribute(Stream, BASSAttribute.BASS_ATTRIB_VOL, Volume / 100F);
                        Bass.BASS_ChannelPlay(Stream, false);
                    }
                }
            }
            else
                Bass.BASS_ChannelPlay(Stream, false);
            isStopped = false;
        }
        protected static void Stop()
        {
            Bass.BASS_ChannelStop(Stream);
            Bass.BASS_StreamFree(Stream);
            isStopped = true;
        }
        protected  bool ToNextTrack()
        {
            if (Bass.BASS_ChannelIsActive(Stream) == BASSActive.BASS_ACTIVE_STOPPED && (!isStopped))
            {
                if (alltrackscount > CurrentTrackNumber + 1)
                {
                    Play(Files[++CurrentTrackNumber], Volume);
                    GetFullMusicInfo.GetTrackInfo();
                    MainWindow.Instance.SetSongInfo();
                    EndPlayList = false;
                    return true;
                }
                else
                    EndPlayList = true;
            }
            return false;
        }
        protected static void PlayRadio(string radioulr, float vol)
        {
            if (Bass.BASS_ChannelIsActive(Stream) != BASSActive.BASS_ACTIVE_PAUSED)
            {
                Stop();
                if (InitBass(HZ))
                {
                    Stream = Bass.BASS_StreamCreateURL(radioulr, 0, BASSFlag.BASS_STREAM_STATUS, null, IntPtr.Zero);
                    if (Stream != 0)
                    {
                        Volume = vol;
                        Bass.BASS_ChannelSetAttribute(Stream, BASSAttribute.BASS_ATTRIB_VOL, Volume / 100F);
                        Bass.BASS_ChannelPlay(Stream, false);
                    }
                }
            }
            else
                Bass.BASS_ChannelPlay(Stream, false);
            isStopped = false;
        }
        #endregion  

        #region Кнопки
        public void PlayFromPlayList(int index)
        {
            Stop();
            string current = Files[index];
            CurrentTrackNumber = index; 
            Play(current, Volume);
            GetFullMusicInfo.GetTrackInfo();
            MainWindow.Instance.SetSongInfo();
            MainWindow.Instance.PlayerStack.Visibility = System.Windows.Visibility.Visible;
            MainWindow.Instance.ProgramName.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.Instance.PlayList.SelectedIndex = index;
        }
        public void PlayButton()
        {
            if (Files.Count == 0)
            {
                DirectoryInfo dr = new DirectoryInfo(Properties.Settings.Default.MusicFolder);
                foreach (var d in dr.GetFiles("*mp3*"))
                {
                    Files.Add(d.FullName);
                }
                alltrackscount = Files.Count;
                CurrentTrackNumber = 0;     
            }
            if (Files.Count != 0)
            {
                PlayListMethods playlistcl = new PlayListMethods();
                timerStart();
                playlistcl.PlayListFill();
                string current = Files[CurrentTrackNumber];
                Play(current, Volume);
                GetFullMusicInfo.GetTrackInfo();
                MainWindow.Instance.PlayerStack.Visibility = System.Windows.Visibility.Visible;
                MainWindow.Instance.ProgramName.Visibility = System.Windows.Visibility.Hidden;
                MainWindow.Instance.Play.Visibility = System.Windows.Visibility.Hidden;
                MainWindow.Instance.Pause.Visibility = System.Windows.Visibility.Visible;
                MainWindow.Instance.TabMusicPlay.Visibility = System.Windows.Visibility.Hidden;
                MainWindow.Instance.TabMusicPause.Visibility = System.Windows.Visibility.Visible;
                MainWindow.Instance.PlayList.SelectedIndex = CurrentTrackNumber;
            }
            MainWindow.Instance.SetSongInfo();
        }
        public void StopButton()
        {
            MainWindow.Instance.Pause.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.Instance.Play.Visibility = System.Windows.Visibility.Visible;
            MainWindow.Instance.TabMusicPlay.Visibility = System.Windows.Visibility.Visible;
            MainWindow.Instance.TabMusicPause.Visibility = System.Windows.Visibility.Hidden;
            if (Bass.BASS_ChannelIsActive(Stream) == BASSActive.BASS_ACTIVE_PLAYING)
                Bass.BASS_ChannelPause(Stream);
        }
        public void NextSongButton()
        {
            Stop();
            if (alltrackscount > CurrentTrackNumber + 1)
            {
                MainWindow.Instance.Play.Visibility = System.Windows.Visibility.Hidden;
                MainWindow.Instance.Pause.Visibility = System.Windows.Visibility.Visible;
                timerStart();
                CurrentTrackNumber += 1;
                string current = Files[CurrentTrackNumber];
                Play(current, Volume);
                GetFullMusicInfo.GetTrackInfo();
                MainWindow.Instance.PlayList.SelectedIndex = CurrentTrackNumber;
            }
            else
            {
                MainWindow.Instance.Play.Visibility = System.Windows.Visibility.Hidden;
                MainWindow.Instance.Pause.Visibility = System.Windows.Visibility.Visible;
                timerStart();
                CurrentTrackNumber = alltrackscount - 1;
                string current = Files[alltrackscount - 1];
                Play(current, Volume);
                GetFullMusicInfo.GetTrackInfo();
                MainWindow.Instance.PlayList.SelectedIndex = CurrentTrackNumber;
            }
            MainWindow.Instance.SetSongInfo();
        }
        public void PreviousSongButton()
        {
            Stop();
            if (CurrentTrackNumber - 1 < 0)
            {
                MainWindow.Instance.Play.Visibility = System.Windows.Visibility.Hidden;
                MainWindow.Instance.Pause.Visibility = System.Windows.Visibility.Visible;
                timerStart();
                CurrentTrackNumber = 0;
                string current = Files[CurrentTrackNumber];
                Play(current, Volume);
                GetFullMusicInfo.GetTrackInfo();
                MainWindow.Instance.PlayList.SelectedIndex = CurrentTrackNumber;   
            }
            else
            {
                MainWindow.Instance.Play.Visibility = System.Windows.Visibility.Hidden;
                MainWindow.Instance.Pause.Visibility = System.Windows.Visibility.Visible;
                timerStart();
                CurrentTrackNumber -= 1;
                string current = Files[CurrentTrackNumber];
                Play(current, Volume);
                GetFullMusicInfo.GetTrackInfo();
                MainWindow.Instance.PlayList.SelectedIndex = CurrentTrackNumber;
            }
           MainWindow.Instance.SetSongInfo();
        }
        public void ExitPlayer()
        {
            Stop();
            timerStop();
            Bass.BASS_StreamFree(Stream);
            MainLogic.MainVars.musicplayagain = 0;
            Files.Clear();
            MainWindow.Instance.PlayerStack.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.Instance.ProgramName.Visibility = System.Windows.Visibility.Visible;
        }
        #endregion

        #region Timer
        private void timerStart()
        {
            dispatcherTimer.Tick += new EventHandler(timerTick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            dispatcherTimer.Start();
        }
        private void timerStop()
        {
            dispatcherTimer.Stop();
        }
        public void timerTick(object sender, EventArgs e)
        {
            MainWindow.Instance.TabTimeNow.Content = TimeSpan.FromSeconds(GetStreamPos(Stream)).ToString();
            MainWindow.Instance.sliderTime.Value = GetStreamPos(Stream);
            MainWindow.Instance.sliderTrack.Value = GetStreamPos(Stream);
            if (ToNextTrack())
            {       
            }
            if (MainWindow.Instance.TabTimeNow.Content.ToString() == "00:01:00")
            {
                var audioFile = TagLib.File.Create(Files[CurrentTrackNumber]);
                if (audioFile.Tag.FirstGenre != null) GetFullMusicInfo.PopularMusicGanre(audioFile.Tag.FirstGenre.ToLower());
            }

            if (EndPlayList){ CurrentTrackNumber = 0; EndPlayList = false; }


        }
        #endregion
    }
}
