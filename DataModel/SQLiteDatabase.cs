using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.Data;
using System.Windows.Forms;

namespace Anime_Quiz.DataModel
{
    // See http://www.dreamincode.net/forums/topic/157830-using-sqlite-with-c%23/
    class SQLiteDatabase
    {
        String dbConnection;
        private SQLiteConnection _sqlConnection;
        private SQLiteCommand _sqlCommand;

        /// <summary>
        ///     Default Constructor for SQLiteDatabase Class.
        /// </summary>
        public SQLiteDatabase()
        {
            dbConnection = "Data Source=animequiz.s3db;Version=3";
        }
        /// <summary>
        ///     Single Param Constructor for specifying the DB file.
        /// </summary>
        /// <param name="inputFile">The File containing the DB</param>
        public SQLiteDatabase(String dbFile)
        {
            dbConnection = String.Format("Data Source={0}", dbFile);
        }
        /// <summary>
        ///     Single Param Constructor for specifying advanced connection options.
        /// </summary>
        /// <param name="connectionOpts">A dictionary containing all desired options and their values</param>
        public SQLiteDatabase(Dictionary<string, string> connectionString)
        {
            String _connectionString = "";
            foreach (KeyValuePair<string, string> row in connectionString)
            {
                _connectionString += String.Format("{0}={1}", row.Key, row.Value);
            }
            _connectionString = _connectionString.Trim().Substring(0, _connectionString.Length - 1);
            dbConnection = _connectionString;
        }

        private void initializeSqlConnection(string sqlCommand)
        {
            _sqlConnection = new SQLiteConnection(dbConnection);
            _sqlConnection.Open();
            _sqlCommand = new SQLiteCommand(_sqlConnection);
            _sqlCommand.CommandText = sqlCommand;
        }
        /// <summary>
        ///     Allows the programmer to run a query against the Database.
        /// </summary>
        /// <param name="sql">The SQL to run</param>
        /// <returns>A DataTable containing the result set.</returns>
        public DataTable getDataTable(string sqlCommand)
        {
            DataTable dataTable = new DataTable();
            try
            {
                initializeSqlConnection(sqlCommand);

                SQLiteDataReader sqlReader = _sqlCommand.ExecuteReader();
                dataTable.Load(sqlReader);

                sqlReader.Close();
                _sqlConnection.Close();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            return dataTable;
        }

        /// <summary>
        ///     Allows the programmer to interact with the database for purposes other than a query.
        /// </summary>
        /// <param name="sql">The SQL to be run.</param>
        /// <returns>An Integer containing the number of rows updated.</returns>
        public int executeNonQuery(string sqlCommand)
        {
            initializeSqlConnection(sqlCommand);

            int rowsUpdated = _sqlCommand.ExecuteNonQuery();

            _sqlConnection.Close();
            return rowsUpdated;
        }

        /// <summary>
        ///     Allows the programmer to retrieve single items from the DB.
        /// </summary>
        /// <param name="sql">The query to run.</param>
        /// <returns>A string.</returns>
        public string executeScalar(string sqlCommand)
        {
            initializeSqlConnection(sqlCommand);

            object value = _sqlCommand.ExecuteScalar();
            _sqlConnection.Close();

            if (value != null)
                return value.ToString();
            return "";
        }

        /// <summary>
        ///     Allows the programmer to easily update rows in the DB.
        /// </summary>
        /// <param name="tableName">The table to update.</param>
        /// <param name="data">A dictionary containing Column names and their new values.</param>
        /// <param name="where">The where clause for the update statement.</param>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        public bool Update(String tableName, Dictionary<String, String> data, String command)
        {
            String vals = "";
            Boolean returnCode = true;
            if (data.Count >= 1)
            {
                foreach (KeyValuePair<String, String> val in data)
                {
                    vals += String.Format(" {0} = '{1}',", val.Key.ToString(), val.Value.ToString());
                }
                vals = vals.Substring(0, vals.Length - 1);
            }
            try
            {
                this.executeNonQuery(String.Format("update {0} set {1} where {2};", tableName, vals, command));
            }
            catch
            {
                returnCode = false;
            }
            return returnCode;
        }

        /// <summary>
        ///     Allows the programmer to easily delete rows from the DB.
        /// </summary>
        /// <param name="tableName">The table from which to delete.</param>
        /// <param name="where">The where clause for the delete.</param>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        public bool Delete(String tableName, String command)
        {
            Boolean returnCode = true;
            try
            {
                this.executeNonQuery(String.Format("delete from {0} where {1};", tableName, command));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                returnCode = false;
            }
            return returnCode;
        }

         /// <summary>
        ///     Allows the programmer to easily insert into the DB
        /// </summary>
        /// <param name="tableName">The table into which we insert the data.</param>
        /// <param name="data">A dictionary containing the column names and data for the insert.</param>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        public bool Insert(String tableName, Dictionary<String, String> data)
        {
            String columns = "";
            String values = "";
            Boolean returnCode = true;
            foreach (KeyValuePair<String, String> val in data)
            {
                columns += String.Format(" {0},", val.Key.ToString());
                values += String.Format(" '{0}',", val.Value);
            }
            columns = columns.Substring(0, columns.Length - 1);
            values = values.Substring(0, values.Length - 1);
            try
            {
                this.executeNonQuery(String.Format("insert into {0}({1}) values({2});", tableName, columns, values));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                returnCode = false;
            }
            return returnCode;
        }

        /// <summary>
        ///     Allows the programmer to easily delete all data from the DB.
        /// </summary>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        public bool ClearDB()
        {
            DataTable tables;
            try
            {
                tables = this.getDataTable("select NAME from SQLITE_MASTER where type='table' order by NAME;");
                foreach (DataRow table in tables.Rows)
                {
                    this.ClearTable(table["NAME"].ToString());
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Allows the user to easily clear all data from a specific table.
        /// </summary>
        /// <param name="table">The name of the table to clear.</param>
        /// <returns>A boolean true or false to signify success or failure.</returns>
        public bool ClearTable(String table)
        {
            try
            {
                this.executeNonQuery(String.Format("delete from {0};", table));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Gets the ID associated with the QuestionSet name
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public int getQuestionSetID(string setName)
        {
            String command = String.Format("select ID from QuestionSets where NAME = '{0}'", setName);
            DataTable result = this.getDataTable(command);
            try
            {
                DataRow row = result.Rows[0];
                return (int)row["id"];
                //return (int)result.Rows[0]["id"];   //never reached
            }
            catch (Exception e)
            {
                return -1;
            }
        }
    }
}
