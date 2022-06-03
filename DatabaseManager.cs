using System.Configuration;
using Microsoft.Data.Sqlite;

namespace CodingTracker
{
    internal class DatabaseManager
    {
        string? ConnectionString = ConfigurationManager.AppSettings.Get("connectionString");

        public void CreateDatabase()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                using (var tableCommand = connection.CreateCommand())
                {
                    connection.Open();
                    tableCommand.CommandText =
                   @"CREATE TABLE IF NOT EXISTS TimeTracker (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    StartTime TEXT,
                    EndTime TEXT,
                    Duration TEXT
                    )";

                    tableCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
