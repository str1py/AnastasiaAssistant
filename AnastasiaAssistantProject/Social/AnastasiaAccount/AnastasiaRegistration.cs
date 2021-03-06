﻿using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AnastasiaAssistantProject.Social.AnastasiaAccount
{
    class AnastasiaRegistration : InitDataBase
    {
        public string RegError { get; set; }
        DbDataReader Reader;
        public bool CheckUserName(string ulogin)
        {
            initializeDB();
            String query = "SELECT login,pass FROM users where login='" + ulogin + "'";
            try
            {
                using (dbCon)
                using (MySqlCommand comm = new MySqlCommand(query, dbCon))
                {
                    dbCon.Open();
                    Reader = comm.ExecuteReader();

                    int count = 0;
                    while (Reader.Read()) { count++; }
                    if (count == 1) { return false; }
                    else if (count == 0)
                    {
                        dbCon.Close();
                        dbCon.Open();
                        comm.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            finally
            {
                dbCon.Close();
            }


            
           


            return false;
        }
        public void Registr(string login, string name, string sname, string pass, string email, int gender, string city, int position, string favourganre, string avatar)
        {
            initializeDB();
            string HasPassword = GetHashString(pass).ToString();
            String query = "INSERT INTO users(login,name,surname,pass,mail,genderID,city,positionsID,favourganre,avatar) VALUES('" + login + "','" + name + "','" + sname + "','" + HasPassword + "','" + email + "','" + gender + "','" + city + "','" + position + "','" + favourganre + "','" + avatar + "')";
            MySqlCommand comm = new MySqlCommand(query, dbCon);

            dbCon.Open();
            comm.ExecuteNonQuery();
            dbCon.Close();
        }

        public string UploadAvatar(string avatarpath, int gender)
        {
            if (String.IsNullOrEmpty(avatarpath) == false)
            {
                string avatarname = Path.GetFileName(avatarpath);
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://waws-prod-bay-029.ftp.azurewebsites.windows.net/Avatars/" + avatarname);
                request.Method = WebRequestMethods.Ftp.UploadFile;

                request.Credentials = new NetworkCredential("str1pyblog", "mSsEtm23NixhpbDTbtCqq2owmRELkHxenMFnNMe5w01gj2yHTB8Qm16ZJ6kT");
                // создаем поток для загрузки файла
                FileStream fs = new FileStream(avatarpath, FileMode.Open);
                byte[] fileContents = new byte[fs.Length];
                fs.Read(fileContents, 0, fileContents.Length);
                fs.Close();
                request.ContentLength = fileContents.Length;

                // пишем считанный в массив байтов файл в выходной поток
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();

                // получаем ответ от сервера в виде объекта FtpWebResponse
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

                response.Close();
                return "ftp://waws-prod-bay-029.ftp.azurewebsites.windows.net/Avatars/" + avatarname;
            }
            else
            {
                switch (gender)
                {
                    case 0:
                        return "ftp://waws-prod-bay-029.ftp.azurewebsites.windows.net/Avatars/famaledefault.jpg";
                    case 1:
                        return "ftp://waws-prod-bay-029.ftp.azurewebsites.windows.net/Avatars/maledefault%20.jpg";
                }
                return null;
            }
        }


    }
}
