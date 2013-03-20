using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Anime_Quiz.Classes
{
    // See http://www.dreamincode.net/forums/topic/157830-using-sqlite-with-c%23/
    class SQLiteDatabase
    {
        String dbConnection;
        private SQLiteConnection _sqlConnection;
        private SQLiteCommand _sqlCommand;
        SQLiteDataAdapter _sqlAdapter;

        /// <summary>
        ///     Default Constructor for SQLiteDatabase Class.
        /// </summary>
        public SQLiteDatabase()
        {
            dbConnection = "Data Source=animequiz.s3db;Version=3";
        }
        public SQLiteDatabase(bool createNew)
        {
            if (createNew)
            {
                SQLiteConnection.CreateFile("animequiz.s3db");
                String createCmd = "CREATE TABLE [Games] ([name] TEXT NOT NULL,[questionSetId] TEXT NOT NULL); CREATE TABLE [QuestionSets] ([name] TEXT  NOT NULL PRIMARY KEY,[type] INTEGER  NOT NULL);"
                    + "CREATE TABLE [Questions] ([id] INTEGER DEFAULT '1' NOT NULL PRIMARY KEY AUTOINCREMENT,[question] TEXT  NOT NULL,[answer] TEXT  NOT NULL,[points] INTEGER DEFAULT '0' NOT NULL,[answered] BOOLEAN DEFAULT '0' NOT NULL,[questionSet] TEXT  NOT NULL,FOREIGN KEY([questionSet]) REFERENCES [QuestionSets]([name]));"
                    + "CREATE TABLE [Scores] ([gameId] TEXT  NOT NULL,[teamId] INTEGER  NOT NULL,[score] INTEGER DEFAULT '0' NOT NULL);"
                    + "CREATE TABLE [Teams] ([id] INTEGER DEFAULT '1' NOT NULL PRIMARY KEY AUTOINCREMENT,[name] TEXT  NOT NULL UNIQUE);";
                this.executeNonQuery(createCmd);
            }
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
        ///     Gets the DataSet from the database command.
        /// </summary>
        /// <param name="command">The SQL command to execute.</param>
        /// <returns>A DataSet</returns>
        public DataSet getDataSet(string command)
        {
            using (_sqlConnection = new SQLiteConnection(dbConnection))
            {
                _sqlConnection.Open();
                DataSet dataSet = new DataSet();
                using (_sqlAdapter = new SQLiteDataAdapter(command, _sqlConnection))
                {
                    _sqlAdapter.Fill(dataSet);
                    return dataSet;
                }
            }
        }

        /// <summary>
        ///     Synchronizes the database with the DataSet
        /// </summary>
        /// <param name="dataSet">The DataSet to synchronize</param>
        /// <param name="tableName">The database table to update</param>
        /// <returns>true if the operation was successful; false otherwise</returns>
        public bool updateDataSet(DataSet dataSet, string tableName)
        {
            using (_sqlConnection = new SQLiteConnection(dbConnection))
            {
                using (_sqlAdapter = new SQLiteDataAdapter("select * from " + tableName, _sqlConnection))
                {
                    using (new SQLiteCommandBuilder(_sqlAdapter))
                    {
                        try
                        {
                            _sqlConnection.Open();
                            _sqlAdapter.Update(dataSet);
                            return true;
                        }
                        catch (Exception crap)
                        {
                            MessageBox.Show(crap.Message);
                            return false;
                        }
                    }
                }
            }
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
        ///     Updates the database by rewriting existing entries.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool InsertOrReplace(String tableName, Dictionary<String, String> data)
        {
            String columns = "";
            String values = "";
            foreach (KeyValuePair<String, String> val in data)
            {
                columns += String.Format(" {0},", val.Key.ToString());
                values += String.Format(" '{0}',", val.Value);
            }
            columns = columns.Substring(0, columns.Length - 1);
            values = values.Substring(0, values.Length - 1);
            try
            {
                this.executeNonQuery(String.Format("insert or replace into {0}({1}) values({2});", tableName, columns, values));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            return true;
        }
        /// <summary>
        ///     Updates the database by rewriting existing entries. This doesn't use quotes,
        ///     allowing us to use SQL commands.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public bool UnquotedInsertOrReplace(String tableName, Dictionary<String, String> data)
        {
            String columns = "";
            String values = "";
            foreach (KeyValuePair<String, String> val in data)
            {
                columns += String.Format(" {0},", val.Key.ToString());
                values += String.Format(" {0},", val.Value);
            }
            columns = columns.Substring(0, columns.Length - 1);
            values = values.Substring(0, values.Length - 1);
            try
            {
                this.executeNonQuery(String.Format("insert or replace into {0}({1}) values({2});", tableName, columns, values));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
            return true;
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
        ///     Deletes the game.
        /// </summary>
        /// <param name="gameId">The name of the game we want to delete</param>
        /// <returns>true if the operation is successful; false otherwise</returns>
        public bool DeleteGame(String gameId)
        {
            try
            {
                this.executeNonQuery(String.Format("delete from Games where name = '{0}'", gameId));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Deletes questions from the given QuestionSet
        /// </summary>
        /// <param name="questionSet">The QuestionSet to clear</param>
        /// <returns>true if the operation is successful; false otherwise.</returns>
        public bool ClearQuestionsFromSet(String questionSet)
        {
            try
            {
                this.executeNonQuery(String.Format("delete from Questions where questionSet = '{0}'", questionSet));
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        ///     Rename the QuestionSet and cascade the changes to the Questions and Games.
        /// </summary>
        /// <param name="oldName">The old QuestionSet name</param>
        /// <param name="newName">The updated QuestionSet name</param>
        /// <returns></returns>
        public bool renameQuestionSet(string oldName, string newName)
        {
            Dictionary<String, String> questionSetData = new Dictionary<String, String>();
            questionSetData.Add("name", newName);
            String questionSetCmd = String.Format("name = '{0}'", oldName);

            Dictionary<String, String> questionData = new Dictionary<string, string>();
            questionData.Add("questionSet", newName);
            String questionDataCmd = String.Format("questionSet = '{0}'", oldName);

            Dictionary<String, String> gameData = new Dictionary<string, string>();
            gameData.Add("questionSetId", newName);
            String gameDataCmd = String.Format("questionSetId = '{0}'", oldName);

            return this.Update("QuestionSets", questionSetData, questionSetCmd) 
                && this.Update("Questions", questionData, questionDataCmd)
                && this.Update("Games", gameData, gameDataCmd);
        }
        public Types getQuestionSetType(string setName)
        {
            String command = String.Format("select TYPE from QuestionSets where NAME = '{0}'", setName);
            DataTable result = this.getDataTable(command);
            try
            {
                return (Types)Convert.ToInt32(result.Rows[0]["type"]);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return Types.Question;
            }
        }
    }
}
