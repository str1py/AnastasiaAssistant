using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnastasiaAssistant.MainLogic.BugsAndCommands
{
    class CoomandBugsMethods:Social.Anastasia.InitDatabase
    {
        public static void SendBugReport(int reason,string discr)
        {
            try
            {
                initializeDB();
                String query = "INSERT INTO bug_report(reason,description,userlogin) VALUES('" + reason + "','" + discr + "','" + Social.Anastasia.AnastasiaLogIn.login + "')";
                MySqlCommand comm = new MySqlCommand(query, dbCon);
                dbCon.Open();
                comm.ExecuteNonQuery();
                dbCon.Close();
                MessageBox.Show("Запрос успешно отправлен");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        public static void SendCommandRequest(string com, string disc)
        {
            try
            {
                initializeDB();
                String query = "INSERT INTO commands_to_add(command,discription,userlogin) VALUES('" + com + "','" + disc + "','" + Social.Anastasia.AnastasiaLogIn.login + "')";
                MySqlCommand comm = new MySqlCommand(query, dbCon);
                dbCon.Open();
                comm.ExecuteNonQuery();
                dbCon.Close();
                MessageBox.Show("Запрос успешно отправлен");
                
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

  
    }
}
