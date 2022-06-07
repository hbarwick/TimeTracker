using System.Configuration;
using System.Data.SQLite;

namespace CodingTracker
{
    public class DatabaseManager
    {

        string? ConnectionString = ConfigurationManager.AppSettings.Get("connectionString");

        /// <summary>
        /// Called on application start. Creates database and tables if not already existing.
        /// </summary>
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

        /// <summary>
        /// Checks the ActiveSession table for flag of whether or not there is an active session.
        /// </summary>
        /// <returns>Bool - True if active session, else false</returns>
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

        /// <summary>
        /// Toggles the active session flag in ActiveSession table.
        /// </summary>
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

        /// <summary>
        /// Writes StartTime, EndTime & Duration of supplied <paramref name="session"/> to the database.
        /// </summary>
        /// <param name="session"></param>
        public void WriteSessionToDatabase(Session session)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                using (var tableCommand = connection.CreateCommand())
                {
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

        /// <summary>
        /// Method <c>RetrieveSessionList</c> Retrieves a List of all Sessions from the database.
        /// Optional limit parameter can apply a limit to the number of results returned.
        /// (Default 10000 value to be higher number than this db ever expected to store)
        /// </summary>
        public List<Session> RetrieveSessionList(int limit = 10000)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                string query = "SELECT * FROM TimeTracker ORDER by Id Desc LIMIT @limit";
                using (var command = new SQLiteCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@limit", limit);
                    List<Session> sessionList = ExecuteDataReader(command);
                    return sessionList;
                }
            }
        }

        /// <summary>
        /// Method <c>RetrieveSessionListByDate</c> Retrieves a List of sessions from the database
        /// with start dates greater or equal than <paramref name="date"/>
        /// </summary>
        public List<Session> RetrieveSessionListByDate(DateTime date)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                string query = "SELECT * FROM TimeTracker WHERE StartTime >= @date";
                using (var command = new SQLiteCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@date", date);
                    List<Session> sessionList = ExecuteDataReader(command);
                    return sessionList;
                }
            }
        }

        /// <summary>
        /// Method <c>RetrieveSessionListByDate</c> Retrieves a List of sessions from the database
        /// with start dates greater or equal than <paramref name="date1"/> 
        /// and lesser or equal than <paramref name="date2"/>
        /// </summary>
        public List<Session> RetrieveSessionListByDateRange(DateTime date1, DateTime date2)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                string query = "SELECT * FROM TimeTracker WHERE StartTime >= @date1 and StartTime <= @date2";
                using (var command = new SQLiteCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@date1", date1);
                    command.Parameters.AddWithValue("@date2", date2);
                    List<Session> sessionList = ExecuteDataReader(command);
                    return sessionList;
                }
            }
        }

        /// <summary>
        /// Method <c>ExecuteDataReader</c> Exectutes the SQLite <paramref name="command"/>'s 
        /// ExecuteReader method, returning a list of Sessions.
        /// </summary>
        private List<Session> ExecuteDataReader(SQLiteCommand command)
        {
            List<Session> sessionList = new();
            using SQLiteDataReader rdr = command.ExecuteReader();
            while (rdr.Read())
            {
                Session session = new Session();
                session.Id = rdr.GetInt32(0);
                session.StartTime = DateTime.Parse(rdr.GetString(1));
                // Null check for endtime as active session will have this field null so cannot parse to string
                if (!rdr.IsDBNull(2))
                    session.EndTime = DateTime.Parse(rdr.GetString(2));
                else
                    session.EndTime = null;
                sessionList.Add(session);
            }
            return sessionList;
        }


        /// <summary>
        /// Method <c>RetrieveAndUpdateSession</c> Called when manual changes are made to start/end times
        /// or when shift is clocked off. Retrieves updated start and end times into a new Session,
        /// then updates the duration column of the row with the auto calculated session duration property.
        /// </summary>
        public void RetrieveAndUpdateSession(int id)
        {
            Session session = new();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                connection.Open();
                string query = "SELECT * FROM TimeTracker WHERE Id = @id";
                using (var command = new SQLiteCommand(query, connection))
                {
                    
                    command.Parameters.AddWithValue("@id", id);
                    using SQLiteDataReader rdr = command.ExecuteReader();
                    while (rdr.Read())
                    {
                        session.Id = rdr.GetInt32(0);
                        session.StartTime = DateTime.Parse(rdr.GetString(1));
                        if (!rdr.IsDBNull(2))
                            session.EndTime = DateTime.Parse(rdr.GetString(2));
                        else
                            session.EndTime = null;
                    }
                }
                using (var tableCommand = connection.CreateCommand())
                {
                    tableCommand.CommandText = "UPDATE TimeTracker SET Duration = @Duration WHERE Id = @Id";
                    tableCommand.Parameters.AddWithValue("@Duration", session.Duration);
                    tableCommand.Parameters.AddWithValue("@Id", id);
                    tableCommand.ExecuteNonQuery();
                }
            }
        }


        /// <summary>
        /// Method <c>RetrieveActiveSessionId</c> Retrieves the integer ID of the active session,
        /// that being the session with NULL end time.
        /// </summary>
        public int RetrieveActiveSessionId()
        {
            int id = 0;
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                string query = "SELECT Id FROM TimeTracker where EndTime IS NULL";
                using (var command = new SQLiteCommand(query, connection))
                {
                    connection.Open();
                    using SQLiteDataReader rdr = command.ExecuteReader();
                    while (rdr.Read())
                    {
                        id = rdr.GetInt32(0);
                    }
                }
            }
            return id;
        }

        /// <summary>
        /// Method <c>DeleteSession</c> Deletes the row from TimeTracker of given int Id.
        /// </summary>
        public void DeleteSession(int id)
        {
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                using (var tableCommand = connection.CreateCommand())
                {
                    connection.Open();
                    tableCommand.CommandText =
                    @"DELETE FROM TimeTracker WHERE Id = @id";
                    tableCommand.Parameters.AddWithValue("@id", id);
                    tableCommand.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Method <c>UpdateSession</c> Updates a row from TimeTracker of given int Id.
        /// Pass col=1 to update StartTime, col=2 to update endtime. Value updated to Datetime value dt.
        /// </summary>
        public void UpdateSession(int id, int col, DateTime dt)
        {
            string sql = string.Empty;
            switch (col)
            {
                case 1:
                    sql = @"UPDATE TimeTracker SET StartTime = @dt WHERE Id = @id";
                    break;
                case 2:
                    sql = @"UPDATE TimeTracker SET EndTime = @dt WHERE Id = @id";
                    break;
            }
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                using (var tableCommand = connection.CreateCommand())
                {
                    connection.Open();
                    tableCommand.CommandText = sql;
                    tableCommand.Parameters.AddWithValue("@id", id);
                    //tableCommand.Parameters.AddWithValue("@columnToUpdate", columnToUpdate);
                    tableCommand.Parameters.AddWithValue("@dt", dt);
                    tableCommand.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Method <c>GetArrayOfIdsAsStrings</c> Selects all Integer IDs from the database and converts 
        /// to string array to be in correct format for UIManager's GetUserInput method.
        /// </summary>
        public string[] GetArrayOfIdsAsStrings()
        {

            List<string> ids = new();
            using (var connection = new SQLiteConnection(ConnectionString))
            {
                string query = "SELECT Id from TimeTracker";
                using (var command = new SQLiteCommand(query, connection))
                {
                    connection.Open();
                    using SQLiteDataReader rdr = command.ExecuteReader();
                    while (rdr.Read())
                    {
                        int intid = rdr.GetInt32(0);
                        string id = intid.ToString();
                        ids.Add(id);
                    }
                }
            }
            // Add 0 to the array as that is the valid menu choice for exiting update/delete option.
            ids.Add("0");
            return ids.ToArray();
        }
    }
}
