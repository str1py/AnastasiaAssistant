using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Diagnostics;
using System.Xml.Linq;
using System.Threading;

namespace AnastasiaAssistantProject.AutoUpdare
{
    class Update: MainAppLogic.OpenFTPconnection
    {
        public bool newversion;
        private string AnastasiaURL ="ftp://waws-prod-bay-029.ftp.azurewebsites.windows.net/AnastasiaAssistantProject/";
        static string path = Directory.GetCurrentDirectory();
        Dictionary<string, Uri> filesinfo = new Dictionary<string, Uri>();

        public string CompareVersions()
        {
            try
            {
                var myVersion = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;

                FtpWebRequest request = ConnectionForDownloadFile(AnastasiaURL , "version.xml");
                XmlDocument doc = new XmlDocument();

                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(responseStream);
                string xml = reader.ReadToEnd();
                doc.LoadXml(xml);

                var versionFromServer = new Version(doc.GetElementsByTagName("version")[0].InnerText);

                Properties.Settings.Default.LastUpdateCheck = DateTime.Now.ToString();
                Properties.Settings.Default.Save();
                if (myVersion < versionFromServer)
                {
                    newversion = true;
                    return "Доступна новая версия - " + versionFromServer;
                }
                else
                {
                    newversion = false;
                    return "У вас актуальная версия приложения";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            return null;
        }
        public string Download()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    client.Credentials = new NetworkCredential(Login, Password);
                    client.DownloadFile(new Uri(AnastasiaURL + "AnastasiaAssistantProject.exe"), "AnastasiaAssistantProject.update");
                
                    client.DownloadFile(new Uri(AnastasiaURL + "Commands.xml"), path + "/Data/" + "Commands.xml");

                    client.DownloadFile(new Uri(AnastasiaURL + "BBCRadioStations.xml"), path + "/Data/" + "BBCRadioStations.xml");

                    client.DownloadFile(new Uri(AnastasiaURL + "MoscowRadioStations.xml"), path + "/Data/" + "MoscowRadioStations.xml");

                    client.DownloadFile(new Uri(AnastasiaURL + "RecordStations.xml"), path + "/Data/" + "RecordStations.xml");

                }
                return "Заканчиваю скачивание...";
            }
            catch(Exception ex)
            {
                return ex.ToString();
            }
   

        }
        public void GetFilesInfo()
        {
            string path = Directory.GetCurrentDirectory();
            XDocument xDoc = XDocument.Load(path+@"/Update/FilesVersions.xml");
            FilesToGetInfo();
            foreach (KeyValuePair<string, Uri> pair in filesinfo)
            {
                var info = (FileVersionInfo.GetVersionInfo(path + @"/"+pair.Key));
                xDoc.Root.Element(pair.Key).Value = info.FileVersion;
            }

            var updinfo = (FileVersionInfo.GetVersionInfo(path + @"/Update/" + "Updater.exe"));
            xDoc.Root.Element("Updater.exe").Value = updinfo.FileVersion;



            xDoc.Save(path + @"/Update/FilesVersions.xml");
        }
        public void CompareFilesVersions()
        {
            XmlDocument localv = new XmlDocument();
            localv.Load(path + @"/Update/FilesVersions.xml");

            FtpWebRequest request = ConnectionForDownloadFile(AnastasiaURL, "version.xml");
            XmlDocument serverv = new XmlDocument();
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string xml = reader.ReadToEnd();
            serverv.LoadXml(xml);
            try
            {
                using (WebClient client = new WebClient())
                {
                    foreach(KeyValuePair<string,Uri> pair in filesinfo)
                    {
                        var versionFromServer = new Version(serverv.GetElementsByTagName(pair.Key)[0].InnerText);
                        var versionFromLocal = new Version(localv.GetElementsByTagName(pair.Key)[0].InnerText);
                        if (versionFromLocal < versionFromServer)
                        {
                            DownloadStatus("Обновляю "+ pair.Key);
                            client.DownloadFile(pair.Value,pair.Key);
                        }
                        else { DownloadStatus("Версия " + pair.Key + " актуальна"); }
                    } 
                    var updverfromserver = new Version(serverv.GetElementsByTagName("Updater.exe")[0].InnerText);
                    var updverfromLocal = new Version(localv.GetElementsByTagName("Updater.exe")[0].InnerText);
                    if (updverfromLocal < updverfromserver)
                    {
                        DownloadStatus("Обновляю " + "Updater.exe");
                        client.DownloadFile(new Uri(AnastasiaURL + "Updater.exe"), path + "/Update/" + "Updater.exe");
                    }
                    else { DownloadStatus("Версия " + "Updater.exe" + " актуальна"); }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public string GetChangelog()
        {
            string htmlCode;
            using (WebClient client = new WebClient())
            {
                client.Credentials = new NetworkCredential(Login, Password);
                htmlCode = client.DownloadString(AnastasiaURL + "releasenotes_ru.html");       
            }
            return htmlCode;
        }
        public async void DownloadStatus(string message)
        {
            await MainWindow.Instance.Downloadlog.Dispatcher.BeginInvoke(new Action(delegate ()
            {
                MainWindow.Instance.Downloadlog.Items.Add(message);
            }));

        }

        void FilesToGetInfo()
        {
            filesinfo.Add("bass.dll", new Uri(AnastasiaURL + "bass.dll"));
            filesinfo.Add("Bass.Net.dll", new Uri(AnastasiaURL + "Bass.Net.dll"));
            filesinfo.Add("BigMath.dll", new Uri(AnastasiaURL + "BigMath.dll"));
            filesinfo.Add("Ionic.ZLib.dll", new Uri(AnastasiaURL + "Ionic.ZLib.dll"));
            filesinfo.Add("Microsoft.Speech.dll", new Uri(AnastasiaURL + "Microsoft.Speech.dll"));
            filesinfo.Add("MySql.Data.dll", new Uri(AnastasiaURL + "MySql.Data.dll"));
            filesinfo.Add("Newtonsoft.Json.dll", new Uri(AnastasiaURL + "Newtonsoft.Json.dll"));
            filesinfo.Add("policy.2.0.taglib-sharp.dll", new Uri(AnastasiaURL + "policy.2.0.taglib-sharp.dll"));
            filesinfo.Add("System.Net.Http.Formatting.dll", new Uri(AnastasiaURL + "System.Net.Http.Formatting.dll"));
            filesinfo.Add("taglib-sharp.dll", new Uri(AnastasiaURL + "taglib-sharp.dll"));
            filesinfo.Add("Telegram.Bot.dll", new Uri(AnastasiaURL + "Telegram.Bot.dll"));
            filesinfo.Add("TeleSharp.TL.dll", new Uri(AnastasiaURL + "TeleSharp.TL.dll"));
            filesinfo.Add("TLSharp.Core.dll", new Uri(AnastasiaURL + "TLSharp.Core.dll"));
            filesinfo.Add("xNet.dll", new Uri(AnastasiaURL + "xNet.dll"));;
        }

    }
}
