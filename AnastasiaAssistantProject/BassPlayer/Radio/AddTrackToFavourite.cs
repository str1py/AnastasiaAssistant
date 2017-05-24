using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AnastasiaAssistantProject.BassPlayer.Radio
{
    class AddTrackToFavourite
    {
        static string path = Directory.GetCurrentDirectory();

    
        public void AddFT(string songname)
        {
            XDocument document = XDocument.Load(path + @"/Data/FavouriteTracks.xml");
            if(File.ReadLines(path + @"/Data/FavouriteTracks.xml").Count()==2)
            {
                int lineCount = File.ReadLines(path + @"/Data/FavouriteTracks.xml").Count() - 1;
                document.Root.Add(new XElement("tracks" + lineCount, songname));
                document.Save(path + @"/Data/FavouriteTracks.xml");
                MainWindow.Instance.PopUpMenuBegin(songname, "добавлен в избранное");
            }else
            {
                int lineCount = File.ReadLines(path + @"/Data/FavouriteTracks.xml").Count() - 2;
                document.Root.Add(new XElement("tracks" + lineCount, songname));
                document.Save(path + @"/Data/FavouriteTracks.xml");
                MainWindow.Instance.PopUpMenuBegin(songname, "добавлен в избранное");
            }
        
        }

        public void ClearFT()
        {
            XDocument document = XDocument.Load(path + @"/Data/FavouriteTracks.xml");
            int lineCount = File.ReadLines(path + @"/Data/FavouriteTracks.xml").Count() - 2;
            for (int i = 1; i <= lineCount-1; i++)
            {
                document.Element("Music").Element("tracks" + i).Remove();
            }
            document.Save(path + @"/Data/FavouriteTracks.xml");
         
        }

        public void GetFTListFromkXML()
        {
            XDocument document = XDocument.Load(path + @"/Data/FavouriteTracks.xml");
            int lineCount = File.ReadLines(path + @"/Data/FavouriteTracks.xml").Count() - 2;
            List<string> tracksfromxml = new List<string>();
            MainAppLogic.MainVars.ftracks.Clear();
            for (int i = 1; i < lineCount; i++)
            {
                var track = document.Root.Elements("tracks"+i).Single().Value;
                tracksfromxml.Add(i+". " + track.ToString());
            }
                
            MainAppLogic.MainVars.ftracks = tracksfromxml;  
        }
    }
}
