using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;

namespace DScan
{
    public class DBConnection
    {
        //string connectionString = "server=ESA-IO;database=mirth;user=scanpdf;password=fHJSFsjd29n1cvSJF;";
        //string connectionString = "server=localhost;database=mirth;user=root;password=;";
        string connectionString = "server=ESA-IO;database=mirth;user=scanpdf;password=fHJSFsjd29n1cvSJF;";
        MySqlConnection mySqlConnection;
        public DBConnection()
        {
            mySqlConnection = new MySqlConnection(connectionString);
        }

        // Verbindungsaufbau mit der Datenbank
        //____________________________________________________________________________

        public bool CheckConnection()
        {
            try
            {
                mySqlConnection.Open();
                mySqlConnection.Close();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Daten in die Datenbank schreiben
        //____________________________________________________________________________

        public void InsertData(DokumentenKategorie doctype,string fallNr, string docBezeichnung, string patVornameNachname)
        {
            string insertsql = "INSERT INTO medico_scanpdf_test (doctype, pat, desxl, cdate, patname) VALUES (@dokumentType, @fallNr, @dokumentBezeichnung, @datum, @patName)";
            mySqlConnection.Open();
            MySqlCommand insertCommand = new MySqlCommand(insertsql, mySqlConnection);
            insertCommand.Parameters.Add("@dokumentType", MySqlDbType.VarChar).Value = doctype;
            insertCommand.Parameters.Add("@fallNr", MySqlDbType.VarChar).Value = fallNr;
            insertCommand.Parameters.Add("@dokumentBezeichnung", MySqlDbType.VarChar).Value = docBezeichnung;
            insertCommand.Parameters.Add("@datum", MySqlDbType.Date).Value = DateTime.Now; // Datum Lokaler Rechner / Zeitzone  // ALTERNative: UTCNow(Globale eindeutige weltzeit +0)
            insertCommand.Parameters.Add("@patName", MySqlDbType.Text).Value = patVornameNachname;

            int test = insertCommand.ExecuteNonQuery();
            mySqlConnection.Close();
            
        }

        // Letzten Eintrag ausgelesen
        //____________________________________________________________________________

            /*
             LAST_INSERTID|
             2342321|
             
             */

        public string SelectData()
        {
            string lastId = "";
            try
            {
                string sql = "SELECT LAST_INSERT_ID();";
                mySqlConnection.Open();
                MySqlCommand command = new MySqlCommand(sql, mySqlConnection);
                MySqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    lastId = dataReader[0].ToString();
                    //MessageBox.Show(dataReader.GetValue(0) + " - " + dataReader.GetValue(1) + " - " + dataReader.GetValue(2) + " - " + dataReader.GetValue(3) + " - " + dataReader.GetValue(4) + " - " + dataReader.GetValue(5));
                }
                dataReader.Close();
                command.Dispose();
                mySqlConnection.Close();
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message); // umfassende Meldung
            }
            return lastId;
        }

        
    }
}
