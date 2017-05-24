using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Net;
using System.IO;
using System.Drawing;
namespace AnastasiaAssistantProject.Social.AnastasiaAccount
{
    class AnastasiaLogIn : InitDataBase
    {
        public static string login;
        public static string name;
        public static string surname;
        public static string mail;
        public static string regdate;
        public static string favourganre;
        public static string gender;
        public static string city;
        public static string avatar;
        public static string position;

        DbDataReader Reader;
        MySqlCommand comm;
        public async Task<Boolean> LogIn(string ulogin, string upassword)
        {

            if (MainAppLogic.MainVars.InternetConnection == true)
            {
                return await Task.Run(() =>
                {
                    try
                    {
                        initializeDB();
                        string haspass = GetHashString(upassword).ToString();
                        String query = "SELECT login,pass FROM users where login='" + ulogin + "' and pass ='" + haspass + "'";
                        comm = new MySqlCommand(query, dbCon);
                        dbCon.Open();
                        Reader = comm.ExecuteReader();
                        int count = 0;
                        while (Reader.Read()) { count++; }
                        dbCon.Close();
                        if (count == 1)
                        {
                            GetUserInfo(ulogin);
                            MainWindow.Instance.A_LogInButton.Dispatcher.Invoke(new Action(() =>
                            {
                                MainWindow.Instance.AnastasiaUserInfo.Content = ulogin;
                            }));
                            comm = null;
                            return true;
                        }
                        else if (String.IsNullOrEmpty(ulogin) || String.IsNullOrEmpty(upassword)) { return false; }
                        else if (String.IsNullOrEmpty(name))
                        {
                            MainWindow.Instance.Errors.Dispatcher.Invoke(new Action(() =>
                            {
                                MainWindow.Instance.Errors.Content = "неверный логин или пароль";
                            }));
                        }
                        else { return false; }
                    }
                    catch (Exception ex)
                    {
                        if (ex.ToString().Contains("Timeout in IO operation"))
                        {
                            MainWindow.Instance.Errors.Dispatcher.Invoke(new Action(() =>
                            {
                                MainWindow.Instance.Errors.Content = "Connection Timeout";
                            }));
                        }
                        else if (ex.ToString().Contains("Не удается прочитать данные из транспортного соединения"))
                        {
                            MainWindow.Instance.Errors.Dispatcher.Invoke(new Action(() =>
                            {
                                MainWindow.Instance.Errors.Content = "Попытка установить соединение была безуспешной";
                            }));
                        }
                    }
                    dbCon.Close();
                    return false;
                });
            }
            else
            {
                await MainWindow.Instance.Errors.Dispatcher.BeginInvoke(new Action(delegate ()
                {
                    MainWindow.Instance.Errors.Content = "Нет подкоючения к сети";
                }));
                return false;
            }



        }
        void Update(string log)
        {
            MainAppLogic.CommandList com = new MainAppLogic.CommandList();
            com.SimpleFavGanre();
            initializeDB();
            String query2 = "Update users SET favourganre ='" + MainAppLogic.MainVars.Genres + "' Where login='" + log + "'";
            MySqlCommand comm = new MySqlCommand(query2, dbCon);
            dbCon.Open();
            DbDataReader Reader = comm.ExecuteReader();
            dbCon.Close();
        }
        async void GetUserInfo(string log)
        {
            initializeDB();
            String query1 = "select u.login, u.name, u.surname, u.mail, g.genderrus, u.city, p.userposition ,u.user_created,u.favourganre,u.avatar From users u Inner Join pos_user p On u.positionsID = p.id Inner Join gender g On u.genderID = g.id where login = '" + log + "'";
            MySqlCommand comm = new MySqlCommand(query1, dbCon);
            await dbCon.OpenAsync();
            comm.ExecuteNonQuery();
            MySqlDataAdapter dataAdapter = new MySqlDataAdapter(comm);
            DataTable dt = new DataTable();
            await dataAdapter.FillAsync(dt);
            var data = dt.Select();
            login = log;
            name = data[0].ItemArray[1] as string;
            surname = (data[0].ItemArray[2]) as string;
            mail = (data[0].ItemArray[3]) as string;
            gender = (data[0].ItemArray[4]) as string;
            city = (data[0].ItemArray[5]) as string;
            position = (data[0].ItemArray[6]) as string;
            regdate = (data[0].ItemArray[7].ToString());
            favourganre = (data[0].ItemArray[8]) as string;
            avatar = (data[0].ItemArray[9]) as string;
            await dbCon.CloseAsync();

        }
        public async void LoadAvatar()
        {
            MainWindow.Instance.AvatarLoadBar.Value += 20;
            byte[] imgbyte = await GetImgByte();
            MainWindow.Instance.AvatarLoadBar.Value += 20;
            await Task.Delay(1500);
            Bitmap avatarbit = await ByteToImage(imgbyte);
            MainWindow.Instance.AvatarLoadBar.Value += 20;
            await Task.Delay(1500);
            ConvertImage(avatarbit);
        }
        async Task<byte[]> GetImgByte()
        {
            WebClient ftpClient = new WebClient();
            ftpClient.Credentials = new NetworkCredential("str1pyblog", "mSsEtm23NixhpbDTbtCqq2owmRELkHxenMFnNMe5w01gj2yHTB8Qm16ZJ6kT");
            MainWindow.Instance.AvatarLoadBar.Value += 10;
            return await ftpClient.DownloadDataTaskAsync(avatar);
        }
        async static Task<Bitmap> ByteToImage(byte[] blob)
        {
            MainWindow.Instance.AvatarLoadBar.Value += 20;
            return await Task.Run(() =>
            {
                using (MemoryStream mStream = new MemoryStream())
                {
                    byte[] pData = blob;

                    mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
                    return new Bitmap(mStream, false);
                }
            });
        }
        void ConvertImage(Bitmap bmp)
        {
            MainWindow.Instance.AvatarLoadBar.Value += 10;
            BitmapSource b = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(bmp.GetHbitmap(), IntPtr.Zero, System.Windows.Int32Rect.Empty, BitmapSizeOptions.FromWidthAndHeight(bmp.Width, bmp.Height));
            MainWindow.Instance.AvatarLoadBar.Visibility = System.Windows.Visibility.Hidden;
            MainWindow.Instance.UserAvatar.Source = b;
        }
    }
}
