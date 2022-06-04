using System.Configuration;
using Microsoft.Data.Sqlite;
using System.Data.SQLite;

namespace CodingTracker
{
    public class DatabaseManager
    {
        string? ConnectionString = ConfigurationManager.AppSettings.Get("connectionString");

        public void CreateDatabase()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
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

                    tableCommand.CommandText =
                    @"CREATE TABLE IF NOT EXISTS ActiveSession (
                    Active INTEGER PRIMARY KEY
                    )";
                    tableCommand.ExecuteNonQuery();

                    // If ActiveSession table is empty, add 0 value for inactive session
                    tableCommand.CommandText =
                    @"INSERT INTO ActiveSession (Active)
                    SELECT 0
                    WHERE NOT EXISTS (SELECT * FROM ActiveSession)
                    ";
                    tableCommand.ExecuteNonQuery();
                }
            }
        }

        public bool CheckForActiveSession()
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                string query = "SELECT Active FROM ActiveSession";
                using (var command = new SQLiteCommand(query, connection))
                {
                    connection.Open();
                    using SQLiteDataReader rdr = command.ExecuteReader();
                    while (rdr.Read())
                    {
                        if (rdr.GetInt32(0) == 0) { return false; }
                        else { return true; }
                    }
                    return false;
                }
            }
        }

        public void ToggleActiveSession()
        {
            int ValueToUpdate = 1;
            if (CheckForActiveSession())
            {
                ValueToUpdate = 0;
            }

            using (var connection = new SQLiteConnection(ConnectionString))
            {
                using (var tableCommand = connection.CreateCommand())
                {
                    connection.Open();

                    tableCommand.CommandText = "UPDATE ActiveSession SET Active = @Value";
                    tableCommand.Parameters.AddWithValue("@Value", ValueToUpdate);
                    tableCommand.ExecuteNonQuery();
                }
            }
        }

        public void WriteSessionToDatabase(Session session)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                using (var tableCommand = connection.CreateCommand())
                {
                    string start = session.StartTime.ToString();
                    connection.Open();
                    tableCommand.CommandText =
                    @"INSERT INTO TimeTracker (StartTime, EndTime, Duration)
                    Values (@StartTime, @EndTime, @Duration)
                    ";

                    tableCommand.Parameters.AddWithValue("@StartTime", session.StartTime);
                    tableCommand.Parameters.AddWithValue("@EndTime", session.EndTime);
                    tableCommand.Parameters.AddWithValue("@Duration", session.Duration);
                    tableCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
