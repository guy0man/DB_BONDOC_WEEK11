using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.DirectoryServices.ActiveDirectory;
using System.Diagnostics;

namespace DB_BONDOC_WEEK11
{
    public static class PetModule
    {
        private static MySqlConnection dbConn;
        private static MySqlCommand sqlCommand;
        private static MySqlDataAdapter da = new MySqlDataAdapter();
        private static DataTable dt;
        private static MySqlDataReader dr;
        private static DataSet ds;
        private static string strConn = "server=localhost; user id=root; database=";
        public static string systemName;
        private static string err;

        public static void _db_Connection()
        {
            try
            {
                dbConn = new MySqlConnection(strConn + "dbms_pet");
                dbConn.Open();
                MessageBox.Show("Test Connection Successful", systemName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dbConn.Close();
            }
            catch (Exception ex)
            {
                err = "Error 101: DBConnection()" + ex.Message;
            }
        }

        public static void _db_Connection(string dbName)
        {
            try
            {
                dbConn = new MySqlConnection(strConn + dbName);
                dbConn.Open();
                MessageBox.Show("Test Connection Successful", systemName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                dbConn.Close();
            }
            catch (Exception ex) 
            {
                err = "Error 101: DBConnection()" + ex.Message;
            }
        }

        public static void _displayRecords(string SQL, DataGridView DG)
        {
            try
            {
                dbConn.Open();
                da = new MySqlDataAdapter(SQL, dbConn);
                dt = new DataTable();
                da.Fill(dt);
                DG.DataSource = dt;
                dbConn.Close();
            }
            catch (Exception ex)
            {
                err = "Error 102: DisplayRecord()" + ex.Message;
            }
        }

        public static DataTable _displayRecords(string SQL)
        {
            try
            {
                dbConn.Open();
                da = new MySqlDataAdapter(SQL, dbConn);
                dt = new DataTable();
                da.Fill(dt);
                dbConn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error 103: displayRecords" + ex.Message);
            }
            return dt;
        }

        public static void _displayRecordsUsingDataReader(string SQL, DataGridView DG)
        {
            try
            {
                dbConn.Open();
                sqlCommand = new MySqlCommand(SQL, dbConn);
                dr = sqlCommand.ExecuteReader();
                DG.DataSource = dr;
                dbConn.Close();
            }
            catch (Exception ex)
            {
                err = "Error 104: displayRecordsUsingDataReader" + ex.Message;
            }
        }

        public static void _displayRecordsUsingDataSet(string SQL, DataGridView DG)
        {
            try
            {
                dbConn.Open();
                sqlCommand = new MySqlCommand(SQL, dbConn);
                da = new MySqlDataAdapter(sqlCommand);
                ds = new DataSet();
                da.Fill(ds);
                DG.DataSource = da;
                dbConn.Close();
            }
            catch (Exception ex)
            {
                err = "Errpr 105: displayRecordsUsingDataSet" + ex.Message;
            }
        }

        public static int _readLastRecord(string SQL, string column)
        {
            int value = 0;
            try
            {
                dbConn.Open();
                da = new MySqlDataAdapter(SQL, dbConn);
                dt = new DataTable();
                da.Fill(dt);
                int count = dt.Rows.Count - 1;
                value = Convert.ToInt32(dt.Rows[count][column]);
                dbConn.Close();
            }
            catch (Exception ex)
            {
                err = "Error 106: ReadLastRecord()" + ex.Message;
            }
            return value;
        }

        public static string readLastRecord(string SQL, string column)
        {
            string value = "";
            try
            {
                dbConn.Open();
                da = new MySqlDataAdapter(SQL, dbConn);
                dt = new DataTable();
                da.Fill(dt);
                int count = dt.Rows.Count - 1;
                value = dt.Rows[count][column].ToString();
                dbConn.Close();
            }
            catch (Exception ex)
            {
                err = "Error 106: ReadLastRecord()" + ex.Message;
            }
            return value;
        }

        public static bool _checkDuplicate(string SQL, string column, string value) 
        {
            bool dup = false;
            try
            {
                dbConn.Open();
                da = new MySqlDataAdapter(SQL, dbConn);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow data in dt.Rows)
                {
                    string strVal = data[column].ToString();
                    if (strVal.ToLower() == value.ToLower())
                    {
                        dup = true;
                        break;
                    }
                }
                dbConn.Close();
            }
            catch(Exception ex)
            {
                err = "Errpr 107: checkDuplicate()" + ex.Message;
            }
            return dup;
        }

        public static bool _isValidAccount(string SQL, string columnUN, string columnPass,string username, string pass) 
        {
            bool valid = false;
            try
            {
                dbConn.Open();
                da = new MySqlDataAdapter(SQL, dbConn);
                dt = new DataTable();
                da.Fill(dt);
                foreach (DataRow data in dt.Rows)
                {
                    string strUsername = data[columnUN].ToString();
                    string strPassword = data[columnPass].ToString();
                    if (strUsername == username && strPassword == pass)
                    {
                        valid = true;
                        break;
                    }
                }
                dbConn.Close();
            }
            catch (Exception ex)
            {
                err = "Error 107: isValid-Login" + ex.Message;
            }
            return valid;
        }

        public static DataTable _readValue(string SQL)
        {
            try
            {
                dbConn.Open();
                da = new MySqlDataAdapter(SQL, dbConn);
                dt = new DataTable();
                da.Fill(dt);
                dbConn.Close();
            }
            catch (Exception ex)
            {
                err = "Errpr 108: readValue()" + ex.Message;
            }
            return dt;
        }

        public static int _recordCount(string tableName)
        {
            int count = 0;
            try
            {
                dbConn.Open();
                da = new MySqlDataAdapter("Select *from " + tableName, dbConn);
                dt = new DataTable();
                da.Fill(dt);
                count = dt.Rows.Count;
                dbConn.Close();
            }
            catch (Exception ex)
            {
                err = "Error 109: RecordCount()" + ex.Message;
            }
            return count;
        }

        public static string _readRecord(string SQL)
        {
            string str = "";
            try
            {
                dbConn.Open();
                da = new MySqlDataAdapter(SQL, dbConn);
                dt = new DataTable();
                da.Fill(dt);
                str = dt.Rows[0][0].ToString();
                dbConn.Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Shoq("Error 103: loadToComboBox" + ex.Message);
            }
            return str;
        }

        public static void _SQLManager(string SQL, string msg)
        {
            try
            {
                dbConn.Open();
                sqlCommand = new MySqlCommand(SQL, dbConn);
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.ExecuteNonQuery();
                dbConn.Close();
                MessageBox.Show("Records succesfully " + msg, systemName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                err = "Error 110: SQLManager()" + ex.Message;
            }
        }

        public static void _SQLManager(string SQL)
        {
            try
            {
                dbConn.Open();
                sqlCommand = new MySqlCommand(SQL, dbConn);
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.ExecuteNonQuery();
                dbConn.Close();           
            }
            catch (Exception ex)
            {
                err = "Error 110: SQLManager()" + ex.Message;
            }
        }

        public static void read(DataGridView DG)
        {
            dbConn.Open();
            sqlCommand = new MySqlCommand("Select *from tbl_pet", dbConn);
            dr = sqlCommand.ExecuteReader();
            string str = "";
            while (dr.Read())
            {
                str = dr.GetValue(1).ToString();
                // MsgBox(str)
            }
            dbConn.Close();
        }

        public static DataTable _loadToComboBox(string SQL)
        {
            try
            {
                dbConn.Open();
                da = new MySqlDataAdapter(SQL, dbConn);
                dt = new DataTable();
                da.Fill(dt);
                dbConn.Close();
            }
            catch (Exception ex)
            {
                err = "Error 111: LoadToComeBox()" + ex.Message;
            }
            return dt;
        }

        public static void _loadToComboBox(string SQL, ComboBox cbo, string display, string value)
        {
            try
            {
                dbConn.Open();
                da = new MySqlDataAdapter(SQL, dbConn);
                dt = new DataTable();
                da.Fill(dt);
                cbo.DataSource = dt;
                cbo.DisplayMember = dt.Columns[display].ToString();
                cbo.ValueMember = dt.Columns[value].ToString();
                dbConn.Close();
            }
            catch (Exception ex)
            {
                err = "Error 112: LoadToComboBox()" + ex.Message;
            }
        }

        public static void _loadToComboBox(string SQL, ComboBox cbo)
        {
            try
            {
                dbConn.Open();
                da = new MySqlDataAdapter(SQL, dbConn);
                dt = new DataTable();
                da.Fill(dt);
                cbo.DataSource = dt;
                cbo.DisplayMember = dt.Columns[0].ToString();
                cbo.ValueMember = dt.Columns[1].ToString();
                dbConn.Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error 103: loadToComboBox" + ex.Message);
            }
        }

        private static void errorMessage(string err)
        {
            try
            {
                MessageBox.Show(err, systemName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("General Error", systemName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

}
