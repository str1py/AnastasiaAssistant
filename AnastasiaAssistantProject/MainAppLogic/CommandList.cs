using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using System.Windows;
using System.IO;
using AnastasiaAssistantProject.BassPlayer;
using AnastasiaAssistantProject.Properties;

namespace AnastasiaAssistantProject.MainAppLogic
{
     class CommandList
    {
        public static Dictionary<string, Action> DicCommands = new Dictionary<string, Action>();
        static string path = Directory.GetCurrentDirectory();

        static XDocument xDoc = XDocument.Load(path + @"/Data/Commands.xml");
        BassNetHelper bass = new BassNetHelper();
        Social.Telegram.BotAnswer bot = new Social.Telegram.BotAnswer();
        private  void Hello()
        {
            AnswersLogic.TempSelect select = new AnswersLogic.TempSelect();
            AnswersLogic.Templates.SimpleAnswer SAData = new AnswersLogic.Templates.SimpleAnswer();
            string str;
            str = xDoc.Root.Element("Hello").Element("Answers").Value;
            string[] randHi = str.Split(',');
            int a = randHi.Count();            Random rnd = new Random();
            MainVars.AnastasiaAnswer = randHi[rnd.Next(randHi.Count())];
            select.SelectTemplate(SAData, null);
        }
        private  void HowAreU()
        {
            AnswersLogic.TempSelect select = new AnswersLogic.TempSelect();
            AnswersLogic.Templates.AnswerWithPict SAData = new AnswersLogic.Templates.AnswerWithPict();
            string str;
            str = xDoc.Root.Element("HowAreU").Element("Answers").Value;
            string[] randHi = str.Split(',');
            Random rnd = new Random();
            MainVars.AnastasiaAnswer = randHi[rnd.Next(randHi.Count())];
            select.SelectTemplate(SAData, null);
        }
        private  void Weather()
        {
            AnswersLogic.TempSelect select = new AnswersLogic.TempSelect();
            AnswersLogic.Templates.AnswerWithPict WAData = new AnswersLogic.Templates.AnswerWithPict();
            AnswersLogic.Weather.GetWeather();
            select.SelectTemplate(WAData, null);
        }

        private  void PlayMusic()
        {
            AnswersLogic.TempSelect select = new AnswersLogic.TempSelect();
            AnswersLogic.Templates.SimpleAnswer SAData = new AnswersLogic.Templates.SimpleAnswer();
            if (MainVars.IsMusicOn == false)
            {
                if (Properties.Settings.Default.MusicFolder == null || String.IsNullOrEmpty(Properties.Settings.Default.MusicFolder))
                {
                    MainVars.AnastasiaAnswer = "Вы не выбрали папку!";
                    select.SelectTemplate(SAData, null);
                }
                else
                {
                    MainWindow.Instance.TabMusicPlay.Visibility = Visibility.Hidden;
                    MainWindow.Instance.TabMusicPause.Visibility = Visibility.Visible;
                    MainWindow.Instance.ChangeToPlayerMethod();
                }
            }
            else
            {
                MainVars.AnastasiaAnswer = "Плеер уже запущен!";
                select.SelectTemplate(SAData, null);
            }
        }
        private  void PlayNextSong()
        {
            AnswersLogic.TempSelect select = new AnswersLogic.TempSelect();
            AnswersLogic.Templates.SimpleAnswer SAData = new AnswersLogic.Templates.SimpleAnswer();
            bass.NextSongButton();
            select.SelectTemplate(SAData, null);
        }
        private  void PlayPrevSong()
        {
            AnswersLogic.TempSelect select = new AnswersLogic.TempSelect();
            AnswersLogic.Templates.SimpleAnswer SAData = new AnswersLogic.Templates.SimpleAnswer();
            bass.PreviousSongButton();
            select.SelectTemplate(SAData, null);
        }
        private  void SongPause()
        {
            AnswersLogic.TempSelect select = new AnswersLogic.TempSelect();
            AnswersLogic.Templates.SimpleAnswer SAData = new AnswersLogic.Templates.SimpleAnswer();
            bass.StopButton();
        }
        private void PlayerExit()//нет комманд
        {
            bass.ExitPlayer();
            MainWindow.Instance.ChangeToPlayer.Visibility = Visibility.Hidden;
            PlayListMethods playlistcl = new PlayListMethods();
            playlistcl.ClearPlayList();
        }
        private void WhatSongNow()//нет комманд
        {
            AnswersLogic.TempSelect select = new AnswersLogic.TempSelect();
            AnswersLogic.Templates.SimpleAnswer SAData = new AnswersLogic.Templates.SimpleAnswer();
            MainVars.AnastasiaAnswer = MainVars.SongName;
            select.SelectTemplate(SAData, null);
        }

        public void PlayRadio()
        {
            if (MainVars.IsRadioOn == false)
            {
                MainWindow.Instance.ChangeToRadio();
            }
            else
            {
                MainVars.AnastasiaAnswer = "Радио уже запущено";
                AnswersLogic.TempSelect select = new AnswersLogic.TempSelect();
                AnswersLogic.Templates.SimpleAnswer SAData = new AnswersLogic.Templates.SimpleAnswer();
                select.SelectTemplate(SAData, null);
            }
        }
        public void StopRadio()
        {
            MainVars.IsRadioOn = false;
            bass.StopUrlStream();
        }
        public void ShowTracksList()
        {
            BassPlayer.Radio.AddTrackToFavourite aft = new BassPlayer.Radio.AddTrackToFavourite();
            aft.GetFTListFromkXML();
            MainWindow.Instance.ShowFTList();
        }
        public void LikeThisTrack()
        {
            if(MainVars.IsRadioOn==true)
            {
                BassPlayer.Radio.AddTrackToFavourite addft = new BassPlayer.Radio.AddTrackToFavourite();
                addft.AddFT(BassPlayer.Radio.StationToPlay.TrackArtist + " - " + BassPlayer.Radio.StationToPlay.TrackName);
            }
        }

        private void FavouriteGanre()
        {
            AnswersLogic.TempSelect select = new AnswersLogic.TempSelect();
            AnswersLogic.Templates.SimpleAnswer SAData = new AnswersLogic.Templates.SimpleAnswer();
            Dictionary<string, int> genres = new Dictionary<string, int>()
            {
                {"House",UserInfo.Default.house}, {"dance",UserInfo.Default.dance}, {"trap",UserInfo.Default.trap},
                 {"electronic",UserInfo.Default.electronic}, {"blues",UserInfo.Default.blues}, {"funky",UserInfo.Default.funky},
                  {"alt. rock",UserInfo.Default.altrock}, {"dubstep",UserInfo.Default.house}, { "moombahton",UserInfo.Default.moombahton},
                    {"trance",UserInfo.Default.house}, {"pop",UserInfo.Default.pop}, {"future bass",UserInfo.Default.futurebass},
                     {"deep house",UserInfo.Default.deephouse}, {"drum & bass",UserInfo.Default.drumbass}, {"rock",UserInfo.Default.rock},
                     {"instrumental",UserInfo.Default.instrumental},{"electro house",UserInfo.Default.electrohouse},{"alternative",UserInfo.Default.alternative},
                     {"glitch hop",UserInfo.Default.glitchhop}, {"hip-hop",UserInfo.Default.hiphop},
            };
            var maxPair = (from d in genres select d.Value).Max();
            string[] Fgenres = new string[3];
            int count = 0;
            foreach (KeyValuePair<string, int> kvp in genres)
            {
                if (count < 3)
                {
                    if (kvp.Value == maxPair && maxPair != 0)
                    {
                        Fgenres[count] = kvp.Key.ToString();
                        count++;
                    }
                }
                if (maxPair == 0)
                {
                    count = 0;
                }
            }
            switch (count)
            {
                case 0:
                    MainVars.AnastasiaAnswer = "К сожалению я не успела понять что вам нравится или вы не слушали музыку вообще.";
                    break;
                case 1:
                    MainVars.AnastasiaAnswer = "Насколько я знаю, Ваш любиый жанр - " + Fgenres[0] + ". Вы прослушали " + maxPair.ToString() + " песен(песни) в этом жанре.";
                    MainVars.Genres = Fgenres[0];
                    break;
                case 2:
                    MainVars.AnastasiaAnswer = "Насколько я знаю, Ваши любиые жанры - " + Fgenres[0] + Fgenres[1] + ". Вы прослушали " + maxPair.ToString() + " песен(песни) в этом жанрах.";
                    MainVars.Genres = Fgenres[0] + Fgenres[1];
                    break;
                case 3:
                    MainVars.AnastasiaAnswer = "Насколько я знаю, Ваши любиые жанры - " + Fgenres[0] + Fgenres[1] + Fgenres[2] + ". Вы прослушали " + maxPair.ToString() + " песен(песни) в этом жанрах.";
                    MainVars.Genres = Fgenres[0] + Fgenres[1] + Fgenres[2];
                    break;
            }

            select.SelectTemplate(SAData, null);

        }//нет комманд
        public void SimpleFavGanre()
        {
            Dictionary<string, int> genres = new Dictionary<string, int>()
            {
                {"House",UserInfo.Default.house}, {"dance",UserInfo.Default.dance}, {"trap",UserInfo.Default.trap},
                 {"electronic",UserInfo.Default.electronic}, {"blues",UserInfo.Default.blues}, {"funky",UserInfo.Default.funky},
                  {"alt. rock",UserInfo.Default.altrock}, {"dubstep",UserInfo.Default.house}, { "moombahton",UserInfo.Default.moombahton},
                    {"trance",UserInfo.Default.house}, {"pop",UserInfo.Default.pop}, {"future bass",UserInfo.Default.futurebass},
                     {"deep house",UserInfo.Default.deephouse}, {"drum & bass",UserInfo.Default.drumbass}, {"rock",UserInfo.Default.rock},
                     {"instrumental",UserInfo.Default.instrumental},{"electro house",UserInfo.Default.electrohouse},{"alternative",UserInfo.Default.alternative},
                     {"glitch hop",UserInfo.Default.glitchhop}, {"hip-hop",UserInfo.Default.hiphop},
            };
            var maxPair = (from d in genres select d.Value).Max();
            string[] Fgenres = new string[3];
            int count = 0;
            foreach (KeyValuePair<string, int> kvp in genres)
            {
                if (count < 3)
                {
                    if (kvp.Value == maxPair && maxPair != 0)
                    {
                        Fgenres[count] = kvp.Key.ToString();
                        count++;
                    }
                }
                if (maxPair == 0)
                {
                    count = 0;
                }
            }
            switch (count)
            {
                case 0:
                    MainVars.Genres = "Информация не собрана";
                    break;
                case 1:
                    MainVars.Genres = Fgenres[0] + " Прослушано - " + maxPair.ToString();
                    break;
                case 2:

                    MainVars.Genres = Fgenres[0] + ", " + Fgenres[1] + " Прослушано - " + maxPair.ToString();
                    break;
                case 3:
                    MainVars.Genres = Fgenres[0] + ", " + Fgenres[1] + ", " + Fgenres[2] + " Прослушано - " + maxPair.ToString();
                    break;
            }
        }

        private void PlayerButTelegram()//нет комманд
        {
            if (Settings.Default.TelegramUserID != "0")
            {
                MainVars.AnastasiaAnswer = "Вот кнопочки";
                bot.ShowPlayerButtons(this, null);
            }
            else
            {
                AnswersLogic.TempSelect select = new AnswersLogic.TempSelect();
                AnswersLogic.Templates.SimpleAnswer SAData = new AnswersLogic.Templates.SimpleAnswer();
                MainVars.AnastasiaAnswer = "Скорее всего вы не авторизованы через Telegram для использование этой команды.";
                select.SelectTemplate(SAData, null);
            }
        }

        public  void AddComands()
        {
            
            AddAnswersFromXAML(Hello,"Hello");
            AddAnswersFromXAML(HowAreU,"HowAreU");
            AddAnswersFromXAML(Weather, "Weather");
            AddAnswersFromXAML(PlayMusic, "PlayMusic");
            AddAnswersFromXAML(PlayNextSong, "PlayNextSong");
            AddAnswersFromXAML(PlayPrevSong, "PlayPrevSong");
            AddAnswersFromXAML(SongPause, "SongPause");
            AddAnswersFromXAML(PlayerExit, "PlayerExit");
            AddAnswersFromXAML(WhatSongNow, "WhatSongNow");
            AddAnswersFromXAML(PlayerButTelegram, "PlayerButTelegram");
            AddAnswersFromXAML(ShowTracksList, "ShowTracksList");
            AddAnswersFromXAML(LikeThisTrack, "LikeThisTrack");
        }
        private static void AddAnswersFromXAML(Action Method,string Name)
        {
            try
            {           
                string str;
                str = xDoc.Root.Element(Name).Element("Command").Value;
                string[] commands = str.Split(',');
                foreach (string word in commands)
                {
                    DicCommands.Add(word, Method);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }
    }
}
