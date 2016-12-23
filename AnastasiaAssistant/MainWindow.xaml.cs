using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using AnastasiaAssistant.BassPlayer;
using System.IO;
using System.Linq;
using System.Text;
using Updater;

namespace AnastasiaAssistant
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region INIT
        public int SetMenuClick;
        MainLogic.CommandListInit main = new MainLogic.CommandListInit();
        public static MainWindow Instance;
        BassPlayer.BassNetHelper bass = new BassPlayer.BassNetHelper();
        MainLogic.Telgram.TelegramAuth auth = new MainLogic.Telgram.TelegramAuth();
        AnswersLogic.TempSelect select = new AnswersLogic.TempSelect();
        AnswersLogic.Templates.SimpleAnswer SAData = new AnswersLogic.Templates.SimpleAnswer();
        BassPlayer.BassNetData songdata = new BassPlayer.BassNetData();
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            Welcome.Window1 well = new Welcome.Window1();
            well.ChackFirsOpen();
            Un4seen.Bass.BassNet.Registration("stripyclear@gmail.com", "2X28183721152222");
            main.AddComands();
            CheckAndInfoAdd();
        
        }

        public void CheckAndInfoAdd()
        {
            DefaultUserName.Text = Properties.Settings.Default.UserName;
            MusicFolder.Text = Properties.Settings.Default.MusicFolder;
            if (Properties.Settings.Default.TelegramUserID != "0")
            {
                MainLogic.Telgram.BotAnswer bot = new MainLogic.Telgram.BotAnswer();
                bot.Main();
                TeleAuth.Visibility = System.Windows.Visibility.Hidden;
                TeleDeAuth.Visibility = System.Windows.Visibility.Visible;
                Uri imagePlayUri = (new Uri(@"pack://application:,,,/AnastasiaRes/Social/t_logo.png", UriKind.Absolute));
                TelegramPicture.Source = new BitmapImage(imagePlayUri);
                MainWindow.Instance.TeleDeAuth.Content = Properties.Settings.Default.TelegramUserName + " " + Properties.Settings.Default.TelegramUserSurName + "\nLogout";
            }
            else
            {
                TeleAuth.Visibility = System.Windows.Visibility.Visible;
                TeleDeAuth.Visibility = System.Windows.Visibility.Hidden;
                Uri imagePlayUri = (new Uri(@"pack://application:,,,/AnastasiaRes/Social/t_logo_notlogin.png", UriKind.Absolute));
                TelegramPicture.Source = new BitmapImage(imagePlayUri);
            }
            Uri imagePlayUri1 = (new Uri(@"pack://application:,,,/AnastasiaRes/Social/AA_logo_Off.png", UriKind.Absolute));
            AnastasiaPicture.Source = new BitmapImage(imagePlayUri1);
            CurrectVersion.Text = "Установленная версия : " + System.Windows.Forms.Application.ProductVersion;

            if (String.IsNullOrEmpty(AnastasiaAssistant.Properties.Settings.Default.LastUpdateCheck))
                LastCheckUpdate.Text = "Последняя проверка : никогда не проводилась";
            else
                LastCheckUpdate.Text = "Последняя проверка : " + AnastasiaAssistant.Properties.Settings.Default.LastUpdateCheck;

            FillCommandList();

            if (String.IsNullOrEmpty(Properties.Settings.Default.changelog) == false)
                changelog.Text = Properties.Settings.Default.changelog;
        }

        #region AppMethods
        static void RestartApp()
        {
            System.Diagnostics.Process.Start(System.Windows.Application.ResourceAssembly.Location);
            System.Windows.Application.Current.Shutdown();
        }
        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        private void AppDown_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void UserMessage_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter) { SendButt_Click(this, new RoutedEventArgs()); }
        }
        private void UserMessage_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case System.Windows.Input.Key.Up:
                    UserMessage.Text = MainLogic.MainVars.UserCommand;
                    break;
                case System.Windows.Input.Key.Down:
                    UserMessage.Text = "";
                    break;
            }
        }
        #endregion

        #region rightMenu
        private void CheckUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            CheckUpdateButton.Visibility = System.Windows.Visibility.Hidden;
            AnastasiaAssistant.AutoUpdare.Update au = new AutoUpdare.Update();
            VersionMessage.Text =  au.CompareVersions();    
            if (au.newversion==true)
            {
                DownloadNowButton.Visibility = System.Windows.Visibility.Visible;
            }
        }
        async private void DownloadNowButton_Click(object sender, RoutedEventArgs e)
        {
            DownloadNowButton.Visibility = System.Windows.Visibility.Hidden;
            DownloadMessages.Visibility = System.Windows.Visibility.Visible;
            downloadPB.Visibility = System.Windows.Visibility.Visible;
            Downloadlog.Visibility = System.Windows.Visibility.Visible;
            AnastasiaAssistant.AutoUpdare.Update au = new AutoUpdare.Update();

            DownloadMessages.Content = "Соединяюсь с сервером...";
            await Task.Delay(3000);
            downloadPB.Value = +20;
  
            DownloadMessages.Content = "Получаю версии файлов...";
            au.GetFilesInfo();
            downloadPB.Value = downloadPB.Value = +20;
            await Task.Delay(3000);

            DownloadMessages.Content = "Сравниваю версии файлов";
            au.CompareFilesVersions();

            DownloadMessages.Content = "Скачиваю файлы...";
            downloadPB.Value = downloadPB.Value + 20;
            await Task.Delay(3000);
       
            DownloadMessages.Content = au.Download();
            downloadPB.Value =downloadPB.Value + 20;
            await Task.Delay(3000);
               
        
            DownloadMessages.Content = "Просматриваю ChangeLog...";
            Properties.Settings.Default.changelog = au.GetChangelog();
            downloadPB.Value = downloadPB.Value + 10;
            await Task.Delay(3000);

            DownloadMessages.Content = "Смотрю список комманд...";
            au.GetCommandList();
            downloadPB.Value = downloadPB.Value + 10;
            await Task.Delay(3000);

            downloadPB.Value = downloadPB.Value + 20;
            DownloadMessages.Content = "Программа перезапустится через 5 секунд...";
            await Task.Delay(5000);

            string path = System.IO.Directory.GetCurrentDirectory();
            Properties.Settings.Default.Save();
        
            System.Diagnostics.Process.Start(path + @"/Updater.exe");
            Environment.Exit(0);

        }
        private void FillCommandList()
        {
            string path = System.IO.Directory.GetCurrentDirectory();
            Encoding enc = Encoding.GetEncoding(1251);
            StreamReader f = new StreamReader(path + @"\Commands.txt",enc);
            string[] a = f.ReadToEnd().Split('\n');
            foreach (var K in a)
            {
                HelpListCommands.Items.Add(K);
            }
        }
        #endregion

        #region SettingsMenu
        private void ShowHideSettingsMenu(string Storyboard, System.Windows.Controls.Button btnHide, System.Windows.Controls.Button btnShow, StackPanel pnl)
        {
            Storyboard sb = Resources[Storyboard] as Storyboard;
            sb.Begin(pnl);

            if (Storyboard.Contains("Show"))
            {
                btnHide.Visibility = System.Windows.Visibility.Visible;
                btnShow.Visibility = System.Windows.Visibility.Visible;
            }
            else if (Storyboard.Contains("Hide"))
            {
                btnShow.Visibility = System.Windows.Visibility.Visible;
            }
        }
        private void SettingsMenuShow_Click(object sender, RoutedEventArgs e)
        {
            ShowHideSettingsMenu("sbSettingsMenuShow", SettingsMenuHide, SettingsMenuShow, SettMenuRight);
     
        }
        private void SettingsMenuHide_Click(object sender, RoutedEventArgs e)
        {
            ShowHideSettingsMenu("sbSettingsMenuHide", SettingsMenuHide, SettingsMenuShow, SettMenuRight);
            SetMenuClick = 0;
        }
        private void ChooseFolderBut_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new FolderBrowserDialog();
            DialogResult result = fbd.ShowDialog();
            MainWindow.Instance.MusicFolder.Text = fbd.SelectedPath;
            Properties.Settings.Default.MusicFolder = MainWindow.Instance.MusicFolder.Text;
        }
        private void SaveButt_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.Language != LanguageChoose.SelectedIndex)
            {
                if (LanguageChoose.SelectedIndex == 0)
                {
                    System.Windows.MessageBox.Show("Для смены языка приложение перезапуститься.");
                    Properties.Settings.Default.Language = 0;
                    Properties.Settings.Default.Save();
                    RestartApp();
                }
                if (LanguageChoose.SelectedIndex == 1)
                {
                    System.Windows.MessageBox.Show("Для смены языка приложение перезапуститься.");
                    Properties.Settings.Default.Language = 1;
                    Properties.Settings.Default.Save();
                    RestartApp();
                }
            }
            if (String.IsNullOrEmpty(DefaultUserName.Text) == false)
            {
                AnastasiaAssistant.Properties.Settings.Default.UserName = DefaultUserName.Text;
                DefaultUserName.Text = Properties.Settings.Default.UserName;
            }
            if (String.IsNullOrEmpty(MusicFolder.Text) == false)
            {
                AnastasiaAssistant.Properties.Settings.Default.MusicFolder = MusicFolder.Text;
                MusicFolder.Text = Properties.Settings.Default.MusicFolder;
            }
            Properties.Settings.Default.Save();
            SettingsMenuHide_Click(this, new RoutedEventArgs());
        }
        private void TeleAuth_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.TelegramUserID == "0")
            {
                MainLogic.Telgram.TelegramAuth tele = new MainLogic.Telgram.TelegramAuth();
                tele.Show();
            }
        }
        private void TeleDeAuth_Click(object sender, RoutedEventArgs e)
        {
            MainLogic.Telgram.BotAnswer bot = new MainLogic.Telgram.BotAnswer();
            bot.StopRec();
            Properties.Settings.Default.TelegramUserID = "0";
            TeleAuth.Visibility = System.Windows.Visibility.Visible;
            TeleDeAuth.Visibility = System.Windows.Visibility.Hidden;
            Properties.Settings.Default.Save();
            Uri imagePlayUri = (new Uri(@"pack://application:,,,/AnastasiaRes/Social/t_logo_notlogin.png", UriKind.Absolute));
            TelegramPicture.Source = new BitmapImage(imagePlayUri);
        }
        #endregion

        #region Answer
        public static int LevenshteinDistance(string string1, string string2)
        {
            if (string1 == null) throw new ArgumentNullException("string1");
            if (string2 == null) throw new ArgumentNullException("string2");
            int diff;
            int[,] m = new int[string1.Length + 1, string2.Length + 1];

            for (int i = 0; i <= string1.Length; i++) { m[i, 0] = i; }
            for (int j = 0; j <= string2.Length; j++) { m[0, j] = j; }

            for (int i = 1; i <= string1.Length; i++)
            {
                for (int j = 1; j <= string2.Length; j++)
                {
                    diff = (string1[i - 1] == string2[j - 1]) ? 0 : 1;

                    m[i, j] = Math.Min(Math.Min(m[i - 1, j] + 1,
                                             m[i, j - 1] + 1),
                                             m[i - 1, j - 1] + diff);
                }
            }
            return m[string1.Length, string2.Length];
        }
        bool SmileMessage()
        {
            if (MainLogic.MainVars.UserCommand == "❤️")
            {
                MainLogic.MainVars.AnastasiaAnswer = "Как мило <3";
                return true;
            }
            else if(MainLogic.MainVars.UserCommand == "😀" || MainLogic.MainVars.UserCommand == "😄" || MainLogic.MainVars.UserCommand == "😂")
            {
                MainLogic.MainVars.AnastasiaAnswer = "Чего смешного-то?";
                return true;
            }
            else if(MainLogic.MainVars.UserCommand == "😘")//поцелуй
            {
                MainLogic.MainVars.AnastasiaAnswer = "И я тебя :*";
                return true;
            }
            else if(MainLogic.MainVars.UserCommand == "😔"|| MainLogic.MainVars.UserCommand == "😩" || MainLogic.MainVars.UserCommand == "😔")//разочарование
            {
                MainLogic.MainVars.AnastasiaAnswer = "Чего это мы надулись?!?!";
                return true;
            }
            else if(MainLogic.MainVars.UserCommand == "😡")//гнев
            {
                MainLogic.MainVars.AnastasiaAnswer = "Смотри не лопни ахахах!!";
                return true;
            }
            else if(MainLogic.MainVars.UserCommand == "☺️" || MainLogic.MainVars.UserCommand == "😊")//улыбка
            {
                MainLogic.MainVars.AnastasiaAnswer = ":)";
                return true;
            }
            else
            {
                return false;
            }
   
        }
        void SearchCommandInDic()
        {
            MainChat.ItemsSource = "";
            MainLogic.MainVars.AnastasiaAnswer = null;
            MainLogic.MainVars.UserCommand = UserMessage.Text;
            bool CommandHasDone = false;
            UserCommandShow.Content = '"' + UserMessage.Text + '"';

            if (Properties.Settings.Default.TelegramUserID != null && CommandHasDone == false)
            {
                CommandHasDone = SmileMessage();//ответ нa смайлы из телеграма
                select.SelectTemplate(SAData, null);
            }

            if (MainLogic.MainVars.UserCommand.Length >=3)
            {
                MainLogic.MainVars.UserCommand = UserMessage.Text.ToLower();
                MainLogic.MainVars.UserCommand.Trim();
                int wordscount = 1;
                string commandToAction = null;
                char[] command = MainLogic.MainVars.UserCommand.ToCharArray();

                for (int i = 0; i < MainLogic.MainVars.UserCommand.Length; i++) { if (command[i] == ' ') { wordscount++; } } //количество слов

                string[] words = MainLogic.MainVars.UserCommand.Split(' ');//слова которые содержаться в запросе

                switch (wordscount)
                {
                    case 1:
                        commandToAction = words[0];
                        break;
                    case 2:
                        commandToAction = words[0] + " " + words[1];
                        break;
                    case 3:
                        commandToAction = words[0] + " " + words[1] + " " + words[2];
                        break;
                }
           
                if (CommandHasDone == false)
                {
                    foreach (KeyValuePair<string, Action> keyValue in main.commands)// Ищем целую команду с погрешностью < 3
                    {
                        string commandFromDic = keyValue.Key.ToString();
                        if (LevenshteinDistance(commandToAction, commandFromDic) < 3) {
                            MainLogic.MainVars.CommandToAction = commandFromDic;
                            if (main.commands.ContainsKey(MainLogic.MainVars.CommandToAction)) {
                                main.commands[MainLogic.MainVars.CommandToAction].Invoke();
                                UserMessage.Clear();                         
                                CommandHasDone = true;
                                break;
                            }
                        }
                    }
                }

                if (CommandHasDone == false)
                {
                    commandToAction = words[0];
                    foreach (KeyValuePair<string, Action> keyValue in main.commands)// Ищем команду по 1ому слову.
                    {
                        string commandFromDic = keyValue.Key.ToString();
                        if (LevenshteinDistance(commandToAction, commandFromDic) < 3) {
                            MainLogic.MainVars.CommandToAction = commandFromDic;
                            if (main.commands.ContainsKey(MainLogic.MainVars.CommandToAction)) {
                                main.commands[MainLogic.MainVars.CommandToAction].Invoke();
                                UserMessage.Clear();
                                CommandHasDone = true;
                                break;
                            }
                        }
                    }
                }

                if (CommandHasDone == false)
                {
                    if (MainLogic.MainVars.UserCommand.Contains("добавить"))
                    {
                        MainLogic.CommandListInit list = new MainLogic.CommandListInit();
                        if (wordscount == 2) { list.AddNewCommands(words[1]); }
                        if (wordscount == 3) { list.AddNewCommands(words[1] + " " + words[2]); }

                        MainLogic.MainVars.AnastasiaAnswer = "Не нашла такой команды((((. Но дабавила в список на добавление)).";
                        select.SelectTemplate(SAData, null);
                        UserMessage.Clear();
                    }
                    else
                    {
                        MainLogic.MainVars.AnastasiaAnswer = "Не нашла такой команды((((. Чтобы добавить в список на добавление введите 'Добавить' и саму команду.";
                        select.SelectTemplate(SAData, null);
                        UserMessage.Clear();
                    }
                }
            }
            else
            {
                if (CommandHasDone == false)
                {
                    MainLogic.MainVars.AnastasiaAnswer = "Нет! Я не умею додумывать. Напишите полную команду, пожалуйста";
                    select.SelectTemplate(SAData, null);
                    UserMessage.Clear();
                }
            }
        }
        public async void SendButt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Task.Run(() =>
                {
                    SendButt.Dispatcher.BeginInvoke(new Action(delegate ()
                    {
                        SearchCommandInDic();
                    }));
                });
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region player
        private void Next_Click(object sender, RoutedEventArgs e)
        {
            bass.NextSongButton();
        }
        private void Prev_Click(object sender, RoutedEventArgs e)
        {
            bass.PreviousSongButton();
        }
        private void Pause_Click(object sender, RoutedEventArgs e)
        {
            bass.StopButton();
        }
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            bass.PlayButton();
        }
        private void sliderTime_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
          // BassPlayer.BassNetHelper.SetPosOfScroll(BassPlayer.BassNetHelper.Stream, sliderTime.Value);
        }
        private void sliderVol_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            BassPlayer.BassNetHelper.SetStreamVolume(BassPlayer.BassNetHelper.Stream, (float)sliderVol.Value);
        }
        #endregion

        #region DataSet
        public void SetSongInfo()
        {
            TabMusicArtist.Content = BassNetData.artist;
            TabMusicSong.Content = BassNetData.songName;
            TabMusicSongPicture.ImageSource = BassNetData.songPicture;
            Artist.Content = BassNetData.artist;
            SongName.Content = BassNetData.songName;
            TabAllTime.Content = BassNetData.songTime;
            sliderTime.Maximum = BassNetData.sliderMaximum;
            sliderTrack.Maximum = BassNetData.sliderMaximum;
        }
        #endregion

        #region TabItem Music
        private void PlayList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            bass.PlayFromPlayList(PlayList.SelectedIndex);
        }
        private void TabMusicNext_Click(object sender, RoutedEventArgs e)
        {
            bass.NextSongButton();
        }
        private void TabMusicPrev_Click(object sender, RoutedEventArgs e)
        {
            bass.PreviousSongButton();
        }
        private void TabMusicPlay_Click(object sender, RoutedEventArgs e)
        {
            bass.PlayButton();
        }
        private void TabMusicPause_Click(object sender, RoutedEventArgs e)
        {
            bass.StopButton();
        }
        private void ResetFavGanre_Click(object sender, RoutedEventArgs e)
        {
            Properties.UserInfo.Default.Reset();
            System.Windows.Forms.MessageBox.Show("Сброс успешно завершен");
        }


        #endregion

        #region Slider
        // private bool _isPressed = false;
        // private Canvas _templateCanvas = null;

        private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Enable moving mouse to change the value.
            //_isPressed = true;
        }

        private void Ellipse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //Disable moving mouse to change the value.
            //_isPressed = false;
        }


        /* private void Ellipse_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
         {
             if (_isPressed)
             {
                 //Find the parent canvas.
                 if (_templateCanvas == null)
                 {
                    // _templateCanvas = MyHelper.FindParent<Canvas>(e.Source as Ellipse);
                     if (_templateCanvas == null) return;
                 }
                 //Canculate the current rotation angle and set the value.
               //  const double RADIUS = 150;
                // Point newPos = e.GetPosition(_templateCanvas);
              //   double angle = MyHelper.GetAngleR(newPos, RADIUS);
               //  sliderTrack.Value = (sliderTrack.Maximum - sliderTrack.Minimum) * angle / (2 * Math.PI);
             }
         }*/
        #endregion
     
        #region AnastasiaRegistration/Login
        private void AnastasiaAuth_Click(object sender, RoutedEventArgs e)
        {
            AnastasiaLogIn.Visibility = System.Windows.Visibility.Visible;
        }
        private void CancleLogIn_Click(object sender, RoutedEventArgs e)
        {
            AnastasiaLogIn.Visibility = System.Windows.Visibility.Hidden;
        }
        private void RegisterBut_Click(object sender, RoutedEventArgs e)
        {
            AnastasiaLogIn.Visibility = System.Windows.Visibility.Hidden;
            AnastasiaRegistration.Visibility = System.Windows.Visibility.Visible;
        }
        private void CancleReg_Click(object sender, RoutedEventArgs e)
        {
            AnastasiaRegistration.Visibility = System.Windows.Visibility.Hidden;
        }
        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(NewLogin.Text) || String.IsNullOrEmpty(NewName.Text) || String.IsNullOrEmpty(NewSurName.Text) ||
                String.IsNullOrEmpty(NewPass.Password) || String.IsNullOrEmpty(NewRepeatPass.Password) || String.IsNullOrEmpty(NewEmail.Text) ||
                String.IsNullOrEmpty(newCity.Text))
            {
                System.Windows.MessageBox.Show("Не все поля заполнены");
            }
            else if (String.IsNullOrEmpty(Sex.Text))
            {
                System.Windows.MessageBox.Show("Не выбран пол");
            }
            else if (NewPass.Password != NewRepeatPass.Password)
            {
                System.Windows.MessageBox.Show("Пароли не совпадают");
            }
            else if (NewEmail.Text.Contains('@'.ToString()) == false)
            {
                System.Windows.MessageBox.Show("Некоректный E-mail");
            }
            else if (NewPass.Password.Length < 7)
            {
                System.Windows.MessageBox.Show("Слишком простой пароль");
            }
            else if (String.IsNullOrEmpty(Position.Text))
            {
                System.Windows.MessageBox.Show("Не выбрана должность");
            }
            else
            {
                MainLogic.Social.Anastasia.AnastasiaRegistration reg = new MainLogic.Social.Anastasia.AnastasiaRegistration();
                string avatar =reg.UploadAvatar(NewAvatar.Text, Sex.SelectedIndex);
                reg.Registr(NewLogin.Text, NewName.Text, NewSurName.Text, NewPass.Password, NewEmail.Text, Sex.SelectedIndex, newCity.Text, Position.SelectedIndex,MainLogic.MainVars.Genres,avatar);
                SuccReg.Visibility = System.Windows.Visibility.Visible;
                AnastasiaRegistration.Visibility = System.Windows.Visibility.Hidden;
            }
        }
        private void FinishReg_Click(object sender, RoutedEventArgs e)
        {
            AnastasiaLogIn.Visibility = System.Windows.Visibility.Hidden;
            AnastasiaRegistration.Visibility = System.Windows.Visibility.Hidden;
            SuccReg.Visibility = System.Windows.Visibility.Hidden;
            SuccLogIn.Visibility = System.Windows.Visibility.Hidden;
        }
        async private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            AnastName.IsEnabled = false;
            AnastPass.IsEnabled = false;
            LogInButton.IsEnabled = false;
            LogInBar.Visibility = System.Windows.Visibility.Visible;
            MainLogic.Social.Anastasia.AnastasiaLogIn login = new MainLogic.Social.Anastasia.AnastasiaLogIn();
            Errors.Content = "Пожалуйста подождите...";
            bool log = false;

            log = await login.LogIn(AnastName.Text, AnastPass.Password);
            if (log == true)
            {
                AnastasiaLogIn.Visibility = Visibility.Hidden;
                SuccLogIn.Visibility = Visibility.Visible;
                AnastPass.Password = null;
                AnastName = null;
                AnastasiaAuth.Visibility = Visibility.Hidden;
                AnastasiaUserInfo.Visibility = Visibility.Visible;
                Uri imagePlayUri = (new Uri(@"pack://application:,,,/AnastasiaRes/Social/AA_logo_On.png", UriKind.Absolute));
                AnastasiaPicture.Source = new BitmapImage(imagePlayUri);
            }
            else
            {
                AnastPass.Password = null;
                AnastName.IsEnabled = true;
                AnastPass.IsEnabled = true;
                LogInButton.IsEnabled = true;
                LogInBar.Visibility = System.Windows.Visibility.Hidden;
            }
        }
        private void AnastasiaUserInfo_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(Uname.Text))
            {
                userinfodb.Visibility = System.Windows.Visibility.Visible;
                Uname.Text = MainLogic.Social.Anastasia.AnastasiaLogIn.name + " " + MainLogic.Social.Anastasia.AnastasiaLogIn.surname;
                Post.Text = MainLogic.Social.Anastasia.AnastasiaLogIn.mail;
                Uregdate.Text = MainLogic.Social.Anastasia.AnastasiaLogIn.regdate;
                Uposition.Text = MainLogic.Social.Anastasia.AnastasiaLogIn.position;
                ufavganre.Text = MainLogic.Social.Anastasia.AnastasiaLogIn.favourganre;
                ucity.Text = MainLogic.Social.Anastasia.AnastasiaLogIn.city;
                ugender.Text = MainLogic.Social.Anastasia.AnastasiaLogIn.gender;
                MainLogic.Social.Anastasia.AnastasiaLogIn log = new MainLogic.Social.Anastasia.AnastasiaLogIn();
                log.LoadAvatar();
            }
            else { userinfodb.Visibility = System.Windows.Visibility.Visible;}
        }
        private void UserInfoExit_Click(object sender, RoutedEventArgs e)
        {
            userinfodb.Visibility = System.Windows.Visibility.Hidden;
        }
        private void ComAddRequest_Click(object sender, RoutedEventArgs e)
        {
            MainLogic.BugsAndCommands.CommandRequest commreq = new MainLogic.BugsAndCommands.CommandRequest();
            commreq.Show();
        }
        private void BugReport_Click(object sender, RoutedEventArgs e)
        {
            MainLogic.BugsAndCommands.BugReport bugreq = new MainLogic.BugsAndCommands.BugReport();
            bugreq.Show();
        }
        private void ChooseAvatar_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();         
            dlg.DefaultExt = ".png";
            dlg.Filter = "JPEG Files (*.jpeg)|*.jpeg|PNG Files (*.png)|*.png|JPG Files (*.jpg)|*.jpg";

            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                NewAvatar.Text = dlg.FileName;
            }
        }


        #endregion

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ResetDefault_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.LastUpdateCheck = "";
            Properties.Settings.Default.changelog = "";
            Properties.Settings.Default.Save();
        }
    }
    public class ValueAngleConverter : IMultiValueConverter
    {
        #region IMultiValueConverter Members

        public object Convert(object[] values, Type targetType, object parameter,
                      System.Globalization.CultureInfo culture)
        {
            double value = (double)values[0];
            double minimum = (double)values[1];
            double maximum = (double)values[2];

            return MyHelper.GetAngle(value, maximum, minimum);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter,
              System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    //Convert the value to text.
    public class ValueTextConverter : IValueConverter
    {

        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
                  System.Globalization.CultureInfo culture)
        {
            double v = (double)value;
            return String.Format("{0:F2}", v);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    public static class MyHelper
    {
        //Get the parent of an item.
        public static T FindParent<T>(FrameworkElement current)
          where T : FrameworkElement
        {
            do
            {
                current = VisualTreeHelper.GetParent(current) as FrameworkElement;
                if (current is T)
                {
                    return (T)current;
                }
            }
            while (current != null);
            return null;
        }

        //Get the rotation angle from the value
        public static double GetAngle(double value, double maximum, double minimum)
        {
            double current = (value / (maximum - minimum)) * 360;
            if (current == 360)
                current = 359.999;

            return current;
        }

        //Get the rotation angle from the position of the mouse
        public static double GetAngleR(Point pos, double radius)
        {
            //Calculate out the distance(r) between the center and the position
            Point center = new Point(radius, radius);
            double xDiff = center.X - pos.X;
            double yDiff = center.Y - pos.Y;
            double r = Math.Sqrt(xDiff * xDiff + yDiff * yDiff);

            //Calculate the angle
            double angle = Math.Acos((center.Y - pos.Y) / r);
            Console.WriteLine("r:{0},y:{1},angle:{2}.", r, pos.Y, angle);
            if (pos.X < radius)
                angle = 2 * Math.PI - angle;
            if (Double.IsNaN(angle))
                return 0.0;
            else
                return angle;
        }

    }
}
