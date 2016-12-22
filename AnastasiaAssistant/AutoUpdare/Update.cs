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

namespace AnastasiaAssistant.AutoUpdare
{
    class Update:AnastasiaAssistant.MainLogic.Social.Anastasia.OpenFTPconnection
    {
        public bool newversion;
        private string AnastasiaURL ="ftp://waws-prod-bay-029.ftp.azurewebsites.windows.net/AnastasiaAssistantProject/";
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
                    client.DownloadFileAsync(new Uri(AnastasiaURL + "AnastasiaAssistant.exe"), "AnastasiaAssistant.update");
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
            string path = System.IO.Directory.GetCurrentDirectory();
            XDocument xDoc = XDocument.Load(path+@"/FilesVersions.xml");
         
            var info = (FileVersionInfo.GetVersionInfo(path + @"/Bass.Net.dll"));
            xDoc.Root.Element("BassNetdll").Value = info.FileVersion;

            info = (FileVersionInfo.GetVersionInfo(path + @"/bass.dll"));
            xDoc.Root.Element("bassdll").Value = info.FileVersion;

            info = (FileVersionInfo.GetVersionInfo(path + @"/BigMath.dll"));
            xDoc.Root.Element("BigMathdll").Value = info.FileVersion;

            info = (FileVersionInfo.GetVersionInfo(path + @"/MySql.Data.dll"));
            xDoc.Root.Element("MySqlDatadll").Value = info.FileVersion;

            info = (FileVersionInfo.GetVersionInfo(path + @"/policy.2.0.taglib-sharp.dll"));
            xDoc.Root.Element("policy20taglibsharpdll").Value = info.FileVersion;

            info = (FileVersionInfo.GetVersionInfo(path + @"/taglib-sharp.dll"));
            xDoc.Root.Element("taglibsharpdll").Value = info.FileVersion;

            info = (FileVersionInfo.GetVersionInfo(path + @"/Telegram.Bot.dll"));
            xDoc.Root.Element("TelegramBotdll").Value = info.FileVersion;

            info = (FileVersionInfo.GetVersionInfo(path + @"/TeleSharp.TL.dll"));
            xDoc.Root.Element("TeleSharpTLdll").Value = info.FileVersion;

            info = (FileVersionInfo.GetVersionInfo(path + @"/TLSharp.Core.dll"));
            xDoc.Root.Element("TLSharpCoredll").Value = info.FileVersion;

            info = (FileVersionInfo.GetVersionInfo(path + @"/Ionic.ZLib.dll"));
            xDoc.Root.Element("lonicZlibdll").Value = info.FileVersion;
            xDoc.Save("FilesVersions.xml");
        }
        public void CompareFilesVersions()
        {
            string path = System.IO.Directory.GetCurrentDirectory();
            XmlDocument localv = new XmlDocument();
            localv.Load(path + @"/FilesVersions.xml");

            FtpWebRequest request = ConnectionForDownloadFile(AnastasiaURL, "version.xml");
            XmlDocument serverv = new XmlDocument();
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string xml = reader.ReadToEnd();
            serverv.LoadXml(xml);


            using (WebClient client = new WebClient())
            {
                var versionFromServer = new Version(serverv.GetElementsByTagName("BassNetdll")[0].InnerText);
                var versionFromLocal = new Version(localv.GetElementsByTagName("BassNetdll")[0].InnerText);
                if (versionFromLocal < versionFromServer)
                {
                    DownloadStatus("Обновляю Bass.Net.dll");
                    client.DownloadFileAsync(new Uri(AnastasiaURL), "Bass.Net.dll");
                }
                else { DownloadStatus("Версия Bass.Net.dll актуальна"); }

                versionFromServer = new Version(serverv.GetElementsByTagName("bassdll")[0].InnerText);
                versionFromLocal = new Version(localv.GetElementsByTagName("bassdll")[0].InnerText);
                if (versionFromLocal < versionFromServer)
                {
                    DownloadStatus("Обновляю bass.dll");
                    client.DownloadFileAsync(new Uri(AnastasiaURL), "bass.dll");
                }
                else { DownloadStatus("Версия bass.dll актуальна"); }

                versionFromServer = new Version(serverv.GetElementsByTagName("BigMathdll")[0].InnerText);
                versionFromLocal = new Version(localv.GetElementsByTagName("BigMathdll")[0].InnerText);
                if (versionFromLocal < versionFromServer)
                {
                    DownloadStatus("Обновляю BigMath.dll");
                    client.DownloadFileAsync(new Uri(AnastasiaURL), "BigMath.dll");
                }
                else { DownloadStatus("Версия BigMath.dll актуальна"); }

                versionFromServer = new Version(serverv.GetElementsByTagName("MySqlDatadll")[0].InnerText);
                versionFromLocal = new Version(localv.GetElementsByTagName("MySqlDatadll")[0].InnerText);
                if (versionFromLocal < versionFromServer)
                {
                    DownloadStatus("Обновляю MySql.Data.dll");
                    client.DownloadFileAsync(new Uri(AnastasiaURL), "MySql.Data.dll");
                }
                else { DownloadStatus("Версия MySql.Data.dll актуальна"); }

                versionFromServer = new Version(serverv.GetElementsByTagName("policy20taglibsharpdll")[0].InnerText);
                versionFromLocal = new Version(localv.GetElementsByTagName("policy20taglibsharpdll")[0].InnerText);
                if (versionFromLocal < versionFromServer)
                {
                    DownloadStatus("Обновляю policy.2.0.taglib-sharp.dll");
                    client.DownloadFileAsync(new Uri(AnastasiaURL), "policy.2.0.taglib-sharp.dll");
                }
                else { DownloadStatus("Версия policy.2.0.taglib-sharp.dll актуальна"); }

                versionFromServer = new Version(serverv.GetElementsByTagName("taglibsharpdll")[0].InnerText);
                versionFromLocal = new Version(localv.GetElementsByTagName("taglibsharpdll")[0].InnerText);
                if (versionFromLocal < versionFromServer)
                {
                    DownloadStatus("Обновляю taglib-sharp.dll");
                    client.DownloadFileAsync(new Uri(AnastasiaURL), "taglib-sharp.dll");
                }
                else { DownloadStatus("Версия taglib-sharp.dll актуальна"); }

                versionFromServer = new Version(serverv.GetElementsByTagName("TelegramBotdll")[0].InnerText);
                versionFromLocal = new Version(localv.GetElementsByTagName("TelegramBotdll")[0].InnerText);
                if (versionFromLocal < versionFromServer)
                {
                    DownloadStatus("Обновляю Telegram.Bot.dll");
                    client.DownloadFileAsync(new Uri(AnastasiaURL), "Telegram.Bot.dll");
                }
                else { DownloadStatus("Версия Telegram.Bot.dll актуальна"); }

                versionFromServer = new Version(serverv.GetElementsByTagName("TeleSharpTLdll")[0].InnerText);
                versionFromLocal = new Version(localv.GetElementsByTagName("TeleSharpTLdll")[0].InnerText);
                if (versionFromLocal < versionFromServer)
                {
                    DownloadStatus("Обновляю TeleSharp.TL.dll");
                    client.DownloadFileAsync(new Uri(AnastasiaURL), "TeleSharp.TL.dll");
                }
                else { DownloadStatus("Версия TeleSharp.TL.dll актуальна"); }

                versionFromServer = new Version(serverv.GetElementsByTagName("TLSharpCoredll")[0].InnerText);
                versionFromLocal = new Version(localv.GetElementsByTagName("TLSharpCoredll")[0].InnerText);
                if (versionFromLocal < versionFromServer)
                {
                    DownloadStatus("Обновляю TLSharp.Core.dll");
                    client.DownloadFileAsync(new Uri(AnastasiaURL), "TLSharp.Core.dll");
                }
                else { DownloadStatus("Версия TLSharp.Core.dll актуальна"); }

                versionFromServer = new Version(serverv.GetElementsByTagName("lonicZlibdll")[0].InnerText);
                versionFromLocal = new Version(localv.GetElementsByTagName("lonicZlibdll")[0].InnerText);
                if (versionFromLocal < versionFromServer)
                {
                    DownloadStatus("Обновляю Ionic.ZLib.dll");
                    client.DownloadFileAsync(new Uri(AnastasiaURL), "Ionic.ZLib.dll");
                }
                else { DownloadStatus("Версия Ionic.ZLib.dll актуальна"); }
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
    }
}
