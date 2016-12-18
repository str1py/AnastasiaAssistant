using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AnastasiaAssistant.MainLogic.Social.Anastasia
{
    public class InitDatabase
    {
        protected const string SERVER = "db4free.net";
        protected const string DATABASE = "anastasia_db";
        protected const string UID = "str1py";
        protected const string PASSWORD = "23nas01devCC";
        protected const uint PORT = 3306;
        protected static MySqlConnection dbCon;
        public static void initializeDB()
        {
            MySqlConnectionStringBuilder builder = new  MySqlConnectionStringBuilder();
            builder.Server = SERVER;
            builder.UserID = UID;
            builder.Password = PASSWORD;
            builder.Database = DATABASE;
            builder.Port = PORT;

            string connString =  builder.ToString();
            builder = null;
            dbCon = new MySqlConnection(connString);

        }
        public Guid GetHashString(string s)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(s);

            MD5CryptoServiceProvider CSP =
                new MD5CryptoServiceProvider();

            byte[] byteHash = CSP.ComputeHash(bytes);

            string hash = string.Empty;
            foreach (byte b in byteHash)
                hash += string.Format("{0:x2}", b);

            return new Guid(hash);
        }

    }
}
