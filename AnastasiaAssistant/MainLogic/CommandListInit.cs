using AnastasiaAssistant.BassPlayer;
using AnastasiaAssistant.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnastasiaAssistant.MainLogic
{
    class CommandListInit
    {
        public Dictionary<string, Action> commands = new Dictionary<string, Action>();
        AnswersLogic.TempSelect select = new AnswersLogic.TempSelect();
        AnswersLogic.Templates.SimpleAnswer SAData = new AnswersLogic.Templates.SimpleAnswer();
        AnswersLogic.Templates.AnswerWithPict WAData = new AnswersLogic.Templates.AnswerWithPict();
        MainLogic.Telgram.BotAnswer bot = new MainLogic.Telgram.BotAnswer();
        BassNetHelper bass = new BassNetHelper();

        private void ansHI()
        {           
            string[] randHi = {"Здравствйте, " + Properties.Settings.Default.UserName, "Рада снова видеть вас " + Properties.Settings.Default.UserName, "Наконец-то вы пришли!" };
            Random rnd = new Random();
            MainVars.AnastasiaAnswer = randHi[rnd.Next(2)];
            select.SelectTemplate(SAData, null);
        }
        private void Weather()
        {
            AnswersLogic.Weather.GetWeather();
            select.SelectTemplate(WAData, null);
        }
        private void dayofweek()
        {
            DateTime day = DateTime.Now;
            MainVars.AnastasiaAnswer = day.DayOfWeek + "," + day.Day;
        }
        private void PlayMusic()
        {
            if (MainVars.musicplayagain == 0)
            {
                if (Properties.Settings.Default.MusicFolder == null || String.IsNullOrEmpty(Properties.Settings.Default.MusicFolder))
                {
                    MainVars.AnastasiaAnswer = "Вы не выбрали папку!";
                    select.SelectTemplate(SAData, null);
                }
                else
                {
                    bass.PlayButton();
                    MainWindow.Instance.MusTabItem.Visibility = System.Windows.Visibility.Visible;
                    MainVars.musicplayagain++;
                }
            }
            else
            {
                MainVars.AnastasiaAnswer = "Плеер уже запущен!";
                select.SelectTemplate(SAData, null);
            }
        }   
        private void PlayNextSong()
        {
            bass.NextSongButton();
            select.SelectTemplate(SAData, null);
        }
        private void PlayPrevSong()
        {
            bass.PreviousSongButton();
            select.SelectTemplate(SAData, null);
        }
        private void SongPause()
        {
            bass.StopButton();
        }
        private void PlayerButTelegram()
        {
            MainLogic.MainVars.AnastasiaAnswer = "Вот кнопочки";
            bot.ShowPlayerButtons(this, null);
        }
        private void PlayerExit()
        {
            bass.ExitPlayer();
            MainWindow.Instance.MusTabItem.Visibility = System.Windows.Visibility.Hidden;
            PlayListMethods playlistcl = new PlayListMethods();
            playlistcl.ClearPlayList();
        }
        private void WhatSongNow()
        {
            MainVars.AnastasiaAnswer = MainVars.SongName;
            select.SelectTemplate(SAData, null);
        }
        private void FavouriteGanre()
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
            string[] Fgenres =new string[3];
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
            switch(count)
            {
                case 0:
                    MainVars.AnastasiaAnswer = "К сожалению я не успела понять что вам нравится или вы не слушали музыку вообще.";
                    break;
                case 1:
                    MainVars.AnastasiaAnswer = "Насколько я знаю, Ваш любиый жанр - " + Fgenres[0]+ ". Вы прослушали " + maxPair.ToString() + " песен(песни) в этом жанре.";
                    MainVars.Genres = Fgenres[0];
                    break;
                case 2:
                    MainVars.AnastasiaAnswer = "Насколько я знаю, Ваши любиые жанры - " + Fgenres[0] + Fgenres[1] +". Вы прослушали " + maxPair.ToString() + " песен(песни) в этом жанрах.";
                    MainVars.Genres = Fgenres[0] + Fgenres[1];
                    break;
                case 3:
                    MainVars.AnastasiaAnswer = "Насколько я знаю, Ваши любиые жанры - " + Fgenres[0] + Fgenres[1]+Fgenres[2] + ". Вы прослушали " + maxPair.ToString() + " песен(песни) в этом жанрах.";
                    MainVars.Genres = Fgenres[0] + Fgenres[1] + Fgenres[2];
                    break;        
            }
          
            select.SelectTemplate(SAData, null);

        }

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
                    MainVars.Genres = Fgenres[0] +" Прослушано - " + maxPair.ToString();
                    break;
                case 2:
                 
                    MainVars.Genres = Fgenres[0]+", " + Fgenres[1] + " Прослушано - " + maxPair.ToString();
                    break;
                case 3:
                    MainVars.Genres = Fgenres[0]+ ", " + Fgenres[1]+ ", " + Fgenres[2] + " Прослушано - " + maxPair.ToString();
                    break;
            }
        }

        #region devhelp
        public void AddNewCommands(string command)
        {
            Properties.UserInfo.Default.NewCommands = new System.Collections.Specialized.StringCollection();
            Properties.UserInfo.Default.NewCommands.Add(command);
            Properties.UserInfo.Default.newcommandcount++;
            Properties.UserInfo.Default.Save();
        }
        public void ShowNewCommands()
        {
            MainLogic.MainVars.AnastasiaAnswer = "Команды которые необходимо добавить ("+ Properties.UserInfo.Default.newcommandcount+") : ";
            for (int i =0;i<Properties.UserInfo.Default.newcommandcount;i++)
            {
                MainLogic.MainVars.AnastasiaAnswer += Properties.UserInfo.Default.NewCommands[i]+" , ";
            }
            select.SelectTemplate(SAData, null);
        }
        #endregion
        public void AddComands()
        {
            commands.Add("привет", ansHI); commands.Add("здрасти", ansHI); commands.Add("здарова", ansHI);
            commands.Add("хай", ansHI); commands.Add("я тут", ansHI); commands.Add("дратути", ansHI);
            commands.Add("hi", ansHI);


            //  commands.Add("дата", num_day);
            // commands.Add("число", dayofweek);
            // commands.Add("время", TimeNow);
            commands.Add("день недели", dayofweek);

            commands.Add("музыка", PlayMusic); commands.Add("включи музыку", PlayMusic); commands.Add("включи плеер", PlayMusic);
         //   commands.Add("радио", Radio); commands.Add("выключи радио", RadioStop);

            commands.Add("след", PlayNextSong); commands.Add("следующий", PlayNextSong);
            commands.Add("пред", PlayPrevSong); commands.Add("предыдущий", PlayPrevSong);
            commands.Add("пауза", SongPause);
            commands.Add("закрой плеер", PlayerExit); commands.Add("останови плеер", PlayerExit); commands.Add("выход из плеер", PlayerExit);
            commands.Add("покажи кнопки", PlayerButTelegram); commands.Add("кнопки плеера", PlayerButTelegram);

            commands.Add("что играет сейчас", WhatSongNow); commands.Add("песня", WhatSongNow);
            commands.Add("что за трэк", WhatSongNow);

            commands.Add("Любимый жанр", FavouriteGanre);
            // commands.Add("громче", volumeUpPlayer);
            // commands.Add("тише", volumeDawnPlayer);

            commands.Add("погода", Weather);


            commands.Add("/newcommands", ShowNewCommands);
          //  commands.Add("/выключи комп", PcOff); commands.Add("/выключи компьютер", PcOff);
           // commands.Add("/перезагрузи комп", PcReboot); commands.Add("/перезагрузи компьютер", PcReboot);
           // commands.Add("/очисти корзину", ClearRecycler);

        }

    }

}
