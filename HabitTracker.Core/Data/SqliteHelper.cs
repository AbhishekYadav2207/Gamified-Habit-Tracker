using System;
using System.Data.SQLite;
using System.IO;

namespace HabitTracker.Core.Data
{
    public class SqliteHelper
    {
        private readonly string _connectionString;

        public SqliteHelper(string databasePath)
        {
            _connectionString = $"Data Source={databasePath};Version=3;";
        }

        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(_connectionString);
        }

        public void ExecuteNonQuery(string query, params SQLiteParameter[] parameters)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public object ExecuteScalar(string query, params SQLiteParameter[] parameters)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                using (var cmd = new SQLiteCommand(query, conn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteScalar();
                }
            }
        }
    }
}
