﻿using System;
using System.Collections.Generic;
using Un4seen.Bass;
using System.IO;
using System.Collections.ObjectModel;
using AnastasiaAssistantProject.MainAppLogic;
using System.Windows;
using System.Runtime.InteropServices;

namespace AnastasiaAssistantProject.BassPlayer
{
    public partial class  BassNetHelper
    { 
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        protected ObservableCollection<PlayList> playlist = new ObservableCollection<PlayList>();

        private static int HZ = 44100;
        private static bool InitDefaultDevice;
        public static int Stream;
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
            Properties.Settings.Default.UserVolume = vol;
            Bass.BASS_ChannelSetAttribute(stream, BASSAttribute.BASS_ATTRIB_VOL, Properties.Settings.Default.UserVolume / 100F);
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
            Bass.BASS_ChannelSetPosition(stream,(double)pos);
        }
        #endregion

        #region Методы кнопок

        protected static void  Play(string track, float vol)
        {
                if (Bass.BASS_ChannelIsActive(Stream) != BASSActive.BASS_ACTIVE_PAUSED)
                {
                    Stop();
                    if (InitBass(HZ))
                    {
                        Stream = Bass.BASS_StreamCreateFile(track, 0, 0, BASSFlag.BASS_DEFAULT);
                        if (Stream != 0)
                        {
                            Properties.Settings.Default.UserVolume = vol;
                            Bass.BASS_ChannelSetAttribute(Stream, BASSAttribute.BASS_ATTRIB_VOL, Properties.Settings.Default.UserVolume / 100F);
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
                    Play(Files[++CurrentTrackNumber], Properties.Settings.Default.UserVolume);
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
        #endregion  

        #region Кнопки
        public void PlayFromPlayList(int index)
        {
            Stop();
            string current = Files[index];
            CurrentTrackNumber = index; 
            Play(current, Properties.Settings.Default.UserVolume);
            GetFullMusicInfo.GetTrackInfo();
            MainWindow.Instance.SetSongInfo();
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
                TimerStart();
                playlistcl.PlayListFill();
                string current = Files[CurrentTrackNumber];
                Play(current, Properties.Settings.Default.UserVolume);
                GetFullMusicInfo.GetTrackInfo();

            }
            MainWindow.Instance.SetSongInfo();
        }
        public void StopButton()
        {
            if (Bass.BASS_ChannelIsActive(Stream) == BASSActive.BASS_ACTIVE_PLAYING)
                Bass.BASS_ChannelPause(Stream);
        }
        public void NextSongButton()
        {
            Stop();
            if (alltrackscount > CurrentTrackNumber + 1)
            {
                TimerStart();
                CurrentTrackNumber += 1;
                string current = Files[CurrentTrackNumber];
                Play(current, Properties.Settings.Default.UserVolume);
                GetFullMusicInfo.GetTrackInfo();
                MainWindow.Instance.PlayList.SelectedIndex = CurrentTrackNumber;
            }
            else
            {
                TimerStart();
                CurrentTrackNumber = alltrackscount - 1;
                string current = Files[alltrackscount - 1];
                Play(current, Properties.Settings.Default.UserVolume);
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
                TimerStart();
                CurrentTrackNumber = 0;
                string current = Files[CurrentTrackNumber];
                Play(current, Properties.Settings.Default.UserVolume);
                GetFullMusicInfo.GetTrackInfo();
                MainWindow.Instance.PlayList.SelectedIndex = CurrentTrackNumber;   
            }
            else
            {
                TimerStart();
                CurrentTrackNumber -= 1;
                string current = Files[CurrentTrackNumber];
                Play(current, Properties.Settings.Default.UserVolume);
                GetFullMusicInfo.GetTrackInfo();
                MainWindow.Instance.PlayList.SelectedIndex = CurrentTrackNumber;
            }
           MainWindow.Instance.SetSongInfo();
        }
        public void ExitPlayer()
        {
            Stop();
            Bass.BASS_StreamFree(Stream);
            MainVars.IsMusicOn = false;
            Files.Clear();
            MainWindow.Instance.PlayList.ItemsSource = null;

            timerStop();
        }
        #endregion

        #region RadioButtons
        public void PlayFromURL(string url, float vol)
        {
            if (Bass.BASS_ChannelIsActive(Stream) != BASSActive.BASS_ACTIVE_PAUSED)
            {
                Stop();
                if (InitBass(HZ))
                {
                    Stream = Bass.BASS_StreamCreateURL(url, 0, BASSFlag.BASS_DEFAULT, null, IntPtr.Zero);
                    if (Stream != 0)
                    {
                        Properties.Settings.Default.UserVolume = vol;
                        Bass.BASS_ChannelSetAttribute(Stream, BASSAttribute.BASS_ATTRIB_VOL, Properties.Settings.Default.UserVolume / 100F);
                        Bass.BASS_ChannelPlay(Stream, false);
                    }
                }
            }
        }
        public void StopUrlStream()
        {
            Stop();
        }

        #endregion

        #region Timer
  
        private void TimerStart()
        {
            dispatcherTimer.Tick += new EventHandler(timerTick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 500);
            dispatcherTimer.Start();
        }
        private void timerStop()
        {   
          dispatcherTimer.Stop();
          dispatcherTimer.IsEnabled = false;
        }
        public void timerTick(object sender, EventArgs e)
        {
            if (MainVars.IsRadioOn == false)
            {
                MainWindow.Instance.TabTimeNow.Content = TimeSpan.FromSeconds(GetStreamPos(Stream)).ToString();
                MainWindow.Instance.sliderTrack.Value = GetStreamPos(Stream);
                if (ToNextTrack())
                {
                }
                if (MainWindow.Instance.TabTimeNow.Content.ToString() == "00:01:00")
                {
                    var audioFile = TagLib.File.Create(Files[CurrentTrackNumber]);
                    if (audioFile.Tag.FirstGenre != null) GetFullMusicInfo.PopularMusicGanre(audioFile.Tag.FirstGenre.ToLower());
                }

                if (EndPlayList) { CurrentTrackNumber = 0; EndPlayList = false; }
            }

        }
        #endregion
    }
}
