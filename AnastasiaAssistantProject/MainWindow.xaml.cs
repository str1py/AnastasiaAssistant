using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using AnastasiaAssistantProject.BassPlayer;
using WinForms = System.Windows.Forms;
using System.Text.RegularExpressions;
using AnastasiaAssistantProject.Social.Telegram;
using System.Data.Common;

namespace AnastasiaAssistantProject
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance;
        MainAppLogic.MainVars mv = new MainAppLogic.MainVars();
        MainAppLogic.SearchAnswer search = new MainAppLogic.SearchAnswer();
        BassNetHelper bass = new BassNetHelper();
        BassPlayer.Radio.StationFillToList fill = new BassPlayer.Radio.StationFillToList();
        BassPlayer.Radio.StationToPlay ps = new BassPlayer.Radio.StationToPlay();
        Social.Telgram.TelegramAuth tauth = new Social.Telgram.TelegramAuth();
        private bool dragStarted = false;
        public int SetMenuClick;

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            Un4seen.Bass.BassNet.Registration("stripyclear@gmail.com", "2X28183721152222");
        }

        public void InitHelper()
        {
            PlayerGrid.Visibility = Visibility.Hidden;
            ChangeToPlayerMessageGrid.Visibility = Visibility.Visible;
            ChangeToRadioMessageGrid.Visibility = Visibility.Visible;
            RadioGrid.Visibility = Visibility.Hidden;
            MainAppLogic.MainVars.IsMusicOn = false;
            MainAppLogic.MainVars.IsRadioOn = false;

            CurrectVersion.Content = "Установленная версия : " + System.Windows.Forms.Application.ProductVersion; 

            if (String.IsNullOrEmpty(Properties.Settings.Default.LastUpdateCheck))
                LastCheckUpdate.Content = "Последняя проверка : никогда не проводилась";
            else
                LastCheckUpdate.Content = "Последняя проверка : " + Properties.Settings.Default.LastUpdateCheck;

            if (String.IsNullOrEmpty(Properties.Settings.Default.changelog) == false)
                changelog.Text = Properties.Settings.Default.changelog;

        }
        public void InitTelegram()
        {
            Uri imagePlayUri1 = (new Uri(@"pack://application:,,,/AnastasiaResources/Social/AA_logo_Off.png", UriKind.Absolute));
            AnastasiaPicture.Source = new BitmapImage(imagePlayUri1);

           MusicFolder.Text = Properties.Settings.Default.MusicFolder;
            if (Properties.Settings.Default.TelegramUserID != "0")
            {
               // BotAnswer bot = new BotAnswer(); ERROR
               // bot.Main();  ERROR
                TeleAuth.Visibility = Visibility.Hidden;
                TeleDeAuth.Visibility = Visibility.Visible;
                Uri imagePlayUri = (new Uri(@"pack://application:,,,/AnastasiaResources/Social/t_logo.png", UriKind.Absolute));
                TelegramPicture.Source = new BitmapImage(imagePlayUri);
                MainWindow.Instance.TeleDeAuth.Content = Properties.Settings.Default.TelegramUserName + " " + Properties.Settings.Default.TelegramUserSurName + "\nLogout";
            }
            else
            {
                TeleAuth.Visibility = Visibility.Visible;
                TeleDeAuth.Visibility = Visibility.Hidden;
                Uri imagePlayUri = (new Uri(@"pack://application:,,,/AnastasiaResources/Social/t_logo_notlogin.png", UriKind.Absolute));
                TelegramPicture.Source = new BitmapImage(imagePlayUri);
            }
        }

        #region SettMenu
        private void ShowHideSettingsMenu(string Storyboard, System.Windows.Controls.Button btnHide, System.Windows.Controls.Button btnShow, Grid pnl)
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
            WinForms.FolderBrowserDialog fbd = new WinForms.FolderBrowserDialog();
            WinForms.DialogResult result = fbd.ShowDialog();
            MusicFolder.Text = fbd.SelectedPath;
            Properties.Settings.Default.MusicFolder = MusicFolder.Text;
        }
        private void SaveButt_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(MusicFolder.Text) == false)
            {
                Properties.Settings.Default.MusicFolder = MusicFolder.Text;
                MusicFolder.Text = Properties.Settings.Default.MusicFolder;
            }
            Properties.Settings.Default.Save();
            SettingsMenuHide_Click(this, new RoutedEventArgs());
        }
        #endregion

        #region PopUpMenu
      
        private void ShowPopUpMenu( Grid pnl2)
        {
            Storyboard sb = Resources["sbPopUpMenuShow"] as Storyboard;
            sb.Begin(pnl2);
        }
        private void HidePopUpMenu( Grid pnl2)
        {
            Storyboard sb = Resources["sbPopUpMenuHide"] as Storyboard;
            sb.Begin(pnl2);
        }
        public async void PopUpMenuBegin(string firstlbl, string secondlbl)
        {
            PopUpLabel1.Content = "";
            PopUpLabel2.Content = "";
            PopUpLabel1.Content = firstlbl;
            PopUpLabel2.Content = secondlbl;

            ShowPopUpMenu(PopUpMenu);
            await Task.Delay(3000);
            HidePopUpMenu(PopUpMenu);

        }

        #endregion

        #region Update
        private void CheckUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            CheckUpdateButton.Visibility = Visibility.Hidden;
            AutoUpdare.Update au = new AutoUpdare.Update();
            VersionMessage.Content = au.CompareVersions();
            if (au.newversion == true)
            {
                DownloadNowButton.Visibility = Visibility.Visible;
            }
        }
        async private void DownloadNowButton_Click(object sender, RoutedEventArgs e)
        {
            DownloadNowButton.Visibility = Visibility.Hidden;
            DownloadMessages.Visibility = Visibility.Visible;
            downloadPB.Visibility = Visibility.Visible;
            Downloadlog.Visibility = Visibility.Visible;
            AutoUpdare.Update au = new AutoUpdare.Update();

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
            downloadPB.Value = downloadPB.Value + 20;
            await Task.Delay(3000);


            DownloadMessages.Content = "Просматриваю ChangeLog...";
            Properties.Settings.Default.changelog = "";
            Properties.Settings.Default.changelog = au.GetChangelog();
            Properties.Settings.Default.Save();
            downloadPB.Value = downloadPB.Value + 20;
            await Task.Delay(3000);

            downloadPB.Value = downloadPB.Value + 20;
            DownloadMessages.Content = "Программа перезапустится через 5 секунд...";
            await Task.Delay(5000);

            string path = System.IO.Directory.GetCurrentDirectory();
            Properties.Settings.Default.Save();

            System.Diagnostics.Process.Start(path + @"/Update/Updater.exe");
            Environment.Exit(0);

        }
        #endregion

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
        private void Maximize_Click(object sender,RoutedEventArgs e)
        {
            if(mv.isWindowMaximaze==false)
            {
                MainWindow.Instance.WindowState = WindowState.Maximized;
                mv.isWindowMaximaze = true;
            }
            else
            {
               MainWindow.Instance.WindowState = WindowState.Normal;
               mv.isWindowMaximaze = false;
            }
        }
        private void UserMessage_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter) { SendButt_Click(this, new RoutedEventArgs()); }
        }
        private void UserMessage_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    UserMessage.Text = MainAppLogic.MainVars.UserCommand;
                    break;
                case Key.Down:
                    UserMessage.Text = "";
                    break;
            }
        }
        #endregion

        #region Answer
        public async void SendButt_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                await Task.Run(() =>
                {
                    SendButt.Dispatcher.BeginInvoke(new Action(delegate ()
                    {
                        MainChat.ItemsSource = "";
                        MainAppLogic.MainVars.AnastasiaAnswer = null;
                        MainAppLogic.MainVars.UserCommand = UserMessage.Text;
                        UserCommand.Content = '"' + UserMessage.Text + '"';
                        MainAppLogic.MainVars.UserCommand = UserMessage.Text.ToLower();
                        search.SearchCommandInDic();
                    }));
                });
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region TabPlayer
        private void PlayList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TabMusicPlay.Visibility = Visibility.Hidden;
            TabMusicPause.Visibility = Visibility.Visible;
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
            TabMusicPlay.Visibility = Visibility.Hidden;
            TabMusicPause.Visibility = Visibility.Visible;
            bass.PlayButton();
        }
        private void TabMusicPause_Click(object sender, RoutedEventArgs e)
        {
            TabMusicPlay.Visibility = Visibility.Visible;
            TabMusicPause.Visibility = Visibility.Hidden;
            bass.StopButton();
        }

        private void ChangeToPlayer_Click(object sender, RoutedEventArgs e)
        {
            ChangeToPlayerMethod();
        }

       public void ChangeToPlayerMethod()
        {
            bass.StopUrlStream();
            PlayerGrid.Visibility = Visibility.Visible;
            ChangeToPlayerMessageGrid.Visibility = Visibility.Hidden;
            ChangeToRadioMessageGrid.Visibility = Visibility.Visible;
            RadioGrid.Visibility = Visibility.Hidden;
            bass.PlayButton();
            MainAppLogic.MainVars.IsMusicOn = true;
            MainAppLogic.MainVars.IsRadioOn = false;
        }
        #endregion

        #region TabRadio
        public void AddStations(string station)
        {
            RadioStationsList.Items.Add(station);
        }
        private void StationsGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RadioStationsList.Items.Clear();
            switch (StationsGroup.SelectedIndex)
            {
                case 0: fill.RecordStationsFill(); break;
                case 1: fill.MoscowStationsFill(); break;
                case 2: fill.BBCStationsFill(); break;
            }
        }
        private void RadioStationsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            PlayRadio.Visibility = Visibility.Hidden;
            ps.Getindex(RadioStationsList.SelectedIndex, StationsGroup.SelectedIndex);
            bass.PlayFromURL(BassPlayer.Radio.StationToPlay.URL,Properties.Settings.Default.UserVolume);
            ps.GetStationInfo();
            SetStationInfo();
            StopRadio.Visibility = Visibility.Visible;
        }
        private void PlayRadio_Click(object sender, RoutedEventArgs e)
        {
            bass.PlayFromURL(BassPlayer.Radio.StationToPlay.URL, Properties.Settings.Default.UserVolume);
            PlayRadio.Visibility = Visibility.Hidden;
            StopRadio.Visibility = Visibility.Visible;
        }
        private void StopRadio_Click(object sender, RoutedEventArgs e)
        {
            PlayRadio.Visibility = Visibility.Visible;
            StopRadio.Visibility = Visibility.Hidden;
            bass.StopUrlStream();
        }

        private void ChangeToRadioButton_Click(object sender, RoutedEventArgs e)
        {
            ChangeToRadio();
        }
        public void ChangeToRadio()
        {
            bass.StopButton();
            PlayerGrid.Visibility = Visibility.Hidden;
            ChangeToPlayerMessageGrid.Visibility = Visibility.Visible;
            ChangeToRadioMessageGrid.Visibility = Visibility.Hidden;
            RadioGrid.Visibility = Visibility.Visible;

            MainAppLogic.MainVars.IsMusicOn = false;
            MainAppLogic.MainVars.IsRadioOn = true;
        }

        public void ShowFTList()
        {
            FTListView.Items.Clear();
            foreach (var track in MainAppLogic.MainVars.ftracks)
                FTListView.Items.Add(track);
            FavouriteTrackList.Visibility = System.Windows.Visibility.Visible;
        }

        private void Like_Click(object sender, RoutedEventArgs e)
        {
            BassPlayer.Radio.AddTrackToFavourite addft = new BassPlayer.Radio.AddTrackToFavourite();
            addft.AddFT(BassPlayer.Radio.StationToPlay.TrackArtist + " - " + BassPlayer.Radio.StationToPlay.TrackName);
        }

        private void CloseFTList_Click(object sender, RoutedEventArgs e)
        {
            FavouriteTrackList.Visibility = System.Windows.Visibility.Hidden;
        }

        private void ClearFTList_Click(object sender, RoutedEventArgs e)
        {
            BassPlayer.Radio.AddTrackToFavourite aft = new BassPlayer.Radio.AddTrackToFavourite();
            aft.ClearFT();
            PopUpMenuBegin("Список очищен","");
            FavouriteTrackList.Visibility = System.Windows.Visibility.Hidden;
        }
        #endregion

        #region DataSet
        public void SetSongInfo()
        {
            TabMusicArtist.Content = BassNetData.artist;
            TabMusicSong.Content = BassNetData.songName;
            TabMusicSongPicture.Source = BassNetData.songPicture;
            TabMusicArtist.Content = BassNetData.artist;
            TabMusicSong.Content = BassNetData.songName;
            TabAllTime.Content = BassNetData.songTime;
            sliderTrack.Maximum = BassNetData.sliderMaximum;
        }

        public void SetStationInfo()
        {
            RadioStationName.Content = BassPlayer.Radio.StationToPlay.StationName;
            TrackArtist.Content = BassPlayer.Radio.StationToPlay.TrackArtist;
            TrackName.Content = BassPlayer.Radio.StationToPlay.TrackName;
            ChannelInfo.Content = BassPlayer.Radio.StationToPlay.ChannelInfo;
        }
        #endregion

        #region AnastasiaLogin
        private void AnastasiaAuth_Click(object sender, RoutedEventArgs e)
        {
            AnastasiaLogIn.Visibility = Visibility.Visible;
        }
        private void CancleLogIn_Click(object sender, RoutedEventArgs e)
        {
            AnastasiaLogIn.Visibility = Visibility.Hidden;
        }
        private async void A_LogInButton_Click(object sender, RoutedEventArgs e)
        {
            AnastName.IsEnabled = false;
            AnastPass.IsEnabled = false;
            A_LogInButton.IsEnabled = false;
            LogInBar.Visibility = Visibility.Visible;
            Social.AnastasiaAccount.AnastasiaLogIn login = new Social.AnastasiaAccount.AnastasiaLogIn();
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
                Uri imagePlayUri = (new Uri(@"pack://application:,,,/AnastasiaResources/Social/AA_logo_On.png", UriKind.Absolute));
                AnastasiaPicture.Source = new BitmapImage(imagePlayUri);
            }
            else
            {
                AnastPass.Password = null;
                AnastName.IsEnabled = true;
                AnastPass.IsEnabled = true;
                A_LogInButton.IsEnabled = true;
                LogInBar.Visibility = System.Windows.Visibility.Hidden;
            }
        }
        private void FinishLogIn_Click(object sender, RoutedEventArgs e)
        {
            AnastasiaLogIn.Visibility = Visibility.Hidden;
            AnastasiaRegistration.Visibility = Visibility.Hidden;
            SuccReg.Visibility = Visibility.Hidden;
            SuccLogIn.Visibility = Visibility.Hidden;
            AnastasiaAuth.Visibility = Visibility.Hidden;
            AnastasiaUserInfo.Visibility = Visibility.Visible;
        }
        private void AnastasiaUserInfo_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(Uname.Text))
            {
                userinfodb.Visibility = Visibility.Visible;
                Uname.Text = Social.AnastasiaAccount.AnastasiaLogIn.name + " " + Social.AnastasiaAccount.AnastasiaLogIn.surname;
                umail.Content = Social.AnastasiaAccount.AnastasiaLogIn.mail;
                Uregdate.Content = Social.AnastasiaAccount.AnastasiaLogIn.regdate;
                Uposition.Content = Social.AnastasiaAccount.AnastasiaLogIn.position;
                ufavganre.Content = Social.AnastasiaAccount.AnastasiaLogIn.favourganre;
                ucity.Content = Social.AnastasiaAccount.AnastasiaLogIn.city;
                ugender.Content = Social.AnastasiaAccount.AnastasiaLogIn.gender;
                Social.AnastasiaAccount.AnastasiaLogIn log = new Social.AnastasiaAccount.AnastasiaLogIn();
                log.LoadAvatar();
            }
            else { userinfodb.Visibility = Visibility.Visible; }
        }
        #endregion

        #region AnastasiaRegistation
        private void CancleReg_Click(object sender, RoutedEventArgs e)
        {
            AnastasiaRegistration.Visibility = System.Windows.Visibility.Hidden;
        }
        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            Social.AnastasiaAccount.AnastasiaRegistration reg = new Social.AnastasiaAccount.AnastasiaRegistration();
            if (reg.CheckUserName(NewLogin.Text) == true)
            {
                if (String.IsNullOrEmpty(NewLogin.Text) || String.IsNullOrEmpty(NewName.Text) || String.IsNullOrEmpty(NewSurName.Text) ||
                    String.IsNullOrEmpty(NewPass.Password) || String.IsNullOrEmpty(NewPassConfirm.Password) || String.IsNullOrEmpty(NewEmail.Text) ||
                    String.IsNullOrEmpty(newCity.Text))
                {
                    RegistrationError.Content="Не все поля заполнены";
                }
                else if (String.IsNullOrEmpty(Sex.Text))
                {
                    RegistrationError.Content="Не выбран пол";
                }
                else if (NewPass.Password != NewPassConfirm.Password)
                {
                    RegistrationError.Content="Пароли не совпадают";
                }
                else if (NewEmail.Text.Contains('@'.ToString()) == false)
                {
                    RegistrationError.Content="Некоректный E-mail";
                }
                else if (NewPass.Password.Length < 7)
                {
                    RegistrationError.Content="Слишком простой пароль";
                }

                else
                {
                    string avatar = reg.UploadAvatar(NewAvatar.Text, Sex.SelectedIndex);
                    reg.Registr(NewLogin.Text, NewName.Text, NewSurName.Text, NewPass.Password, NewEmail.Text, Sex.SelectedIndex, newCity.Text, 3, MainAppLogic.MainVars.Genres, avatar);
                    SuccReg.Visibility = Visibility.Visible;
                    AnastasiaRegistration.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                RegistrationError.Content = "Такое имя уже существует.";
            }       
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
        private void A_RegisterBut_Click(object sender, RoutedEventArgs e)
        {
            AnastasiaLogIn.Visibility = System.Windows.Visibility.Hidden;
            AnastasiaRegistration.Visibility = System.Windows.Visibility.Visible;
        }
        private void FinishReg_Click(object sender, RoutedEventArgs e)
        {
            SuccReg.Visibility = Visibility.Hidden;
        }
        private void UserInfoExit_Click(object sender, RoutedEventArgs e)
        {
            userinfodb.Visibility = Visibility.Hidden;
        }
        #endregion

        #region Telegram
       
        private void TeleAuth_Click(object sender, RoutedEventArgs e)
        {
            TelegramAuthGrid.Visibility = Visibility.Visible;
        }
        private async void TeleNextButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessage.Content = null;
            tauth.ErrorMessage = null;
            await tauth.PhoneCheck(TelegramPhone.Text);
            if (String.IsNullOrEmpty(tauth.ErrorMessage) == true)
            {
                char[] phone = TelegramPhone.Text.ToCharArray();
                UserPhoneNumber.Content = "+" + phone[0] + " (" + phone[1] + phone[2] + phone[3] + ") " + phone[4] + phone[5] + phone[6] + "-" + phone[7] + phone[8] + "-" + phone[9] + phone[10];
                TelegramAuthGrid.Visibility = Visibility.Hidden;
                TelegramCodeGrid.Visibility = Visibility.Visible;
            }
            else
            {
                ErrorMessage.Content = tauth.ErrorMessage;
            }
        }
        private async void WaterMarkCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(WaterMarkCode.Text.Length ==5)
            {
                await tauth.CodeCheck(WaterMarkCode.Text);
                if(MainAppLogic.MainVars.isTelegramLogicSeccused == true)
                {
                    TelegramCodeGrid.Visibility = Visibility.Hidden;
                    TelegraAuthMessage.Visibility = Visibility.Visible;
                    TeleAuthMessageLabel.Content = "Авторизация прошла успешно";
                    await Task.Delay(1000);
                    TelegraAuthMessage.Visibility = Visibility.Hidden;
                    Uri imagePlayUri = (new Uri(@"pack://application:,,,/AnastasiaResources/Social/t_logo.png", UriKind.Absolute));
                    TelegramPicture.Source = new BitmapImage(imagePlayUri);
                    TeleAuth.Visibility = Visibility.Hidden;
                    TeleDeAuth.Visibility = Visibility.Visible;
                    TeleDeAuth.Content = Properties.Settings.Default.TelegramUserName + " " + Properties.Settings.Default.TelegramUserSurName + "\nLogout";
                    BotAnswer bot = new BotAnswer();
                    bot.Main();
                    bot.TestMessage();

                }else
                {
                    WaterMarkCode.Text = null;
                    CodeError.Content = tauth.ErrorMessage;
                }
            }
   
        }

        private void CancleTeleAuth_Click(object sender, RoutedEventArgs e)
        {
            TelegramAuthGrid.Visibility = Visibility.Hidden;
        }
        private void TeleDeAuth_Click(object sender, RoutedEventArgs e)
        {
            BotAnswer bot = new BotAnswer();
            bot.StopRec();
            Properties.Settings.Default.TelegramUserID = "0";
            TeleAuth.Visibility = Visibility.Visible;
            TeleDeAuth.Visibility = Visibility.Hidden;
            Properties.Settings.Default.Save();
            Uri imagePlayUri = (new Uri(@"pack://application:,,,/AnastasiaResources/Social/t_logo_notlogin.png", UriKind.Absolute));
            TelegramPicture.Source = new BitmapImage(imagePlayUri);
        }
  
        #endregion


        #region SliderPlayer

        private void sliderTrack_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (dragStarted==true)
                BassNetHelper.SetPosOfScroll(BassNetHelper.Stream, sliderTrack.Value);
        }

        private void sliderTrack_DragStarted(object sender, RoutedEventArgs e)
        {
            this.dragStarted = true;
        }

        private void sliderTrack_DragCompleted(object sender, RoutedEventArgs e)
        {
            this.dragStarted = false;
        }

        private void SliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            BassNetHelper.SetStreamVolume(BassNetHelper.Stream, (float)SliderVolume.Value);
            Properties.Settings.Default.UserVolume = (float)SliderVolume.Value;
        }


        #endregion
        #region SliderRadio
        private void SliderVolumeRadio_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            BassNetHelper.SetStreamVolume(BassNetHelper.Stream, (float)SliderVolumeRadio.Value);
            Properties.Settings.Default.UserVolume = (float)SliderVolumeRadio.Value;
        }

        #endregion

        private void changelog_TextChanged(object sender, TextChangedEventArgs e)
        {

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
