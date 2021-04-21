using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Data.Sqlite;

namespace SQLite_Backend
{
    public class DataAccess
    {
        private readonly string dbpath;
        private readonly string password;

        private string connectionString;

        public DataAccess(string _dbpath, string _password = "HelloWorld")
        {
            dbpath = _dbpath;
            password = _password;
            connectionString = new SqliteConnectionStringBuilder
            {
                DataSource = dbpath,
                Password = password,
                ForeignKeys = false // Enable or disable whenever you wanna use it or not.
            }.ToString();
        }

        public bool CreateDatabase()
        {
            try
            {
                if (File.Exists(dbpath))
                {
                    Console.WriteLine("Database already exists.");
                    return false;
                }

                using (SqliteConnection con = new SqliteConnection(connectionString)) // Creating a new SqliteConnection on a (big) encrypted database might take some time, if there are many database calls, the connection should be made once and kept in memory.
                using (SqliteCommand cmd = new SqliteCommand
                (
                    commandText:"CREATE TABLE[TestTable] (" +
                                  "[Id] INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL," +
                                  "[Data] text NULL);",
                    connection:con
                ))
                {
                    con.Open();

                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Database created.");
                    return true;
                }
            }
            catch (Exception e)
            {
                throw e;
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public string[] RunQuery(string query)
        {
            try
            {
                List<string> results = new List<string>();
                using (SqliteConnection con = new SqliteConnection(connectionString))
                using (SqliteCommand cmd = new SqliteCommand(
                    commandText: query,
                    connection: con))
                {
                    con.Open();

                    using (SqliteDataReader reader = cmd.ExecuteReader())
                        while (reader.Read())
                        {
                            object[] res = new object[reader.FieldCount];
                            reader.GetValues(res);
                            results.Add(string.Join("\t", res));
                        }
                    return results.ToArray();
                }
            }
            catch (Exception e)
            {
                throw e;
                Console.WriteLine(e.Message);
                return new string[] { "Error" };
            }
        }
    }
}
