using ConsoleTableExt;

namespace CodingTracker
{
    public class Reports
    {
        /// <summary>
        /// Prints out the Id, StartTime, EndTime & Duration of the supplied list of sessions.
        /// Formats into table using ConsoleTableExt for prettier display.
        /// </summary>
        /// <param name="sessionList">List of Session objects.</param>
        public static void DisplayAllRecords(List<Session> sessionList)
        {
            var tableData = new List<List<object>>();

            foreach (Session session in sessionList)
            {
                tableData.Add(new List<object> { session.Id, session.StartTime, session.EndTime, session.Duration.ToString(@"hh\:mm\:ss") });
            }
            ConsoleTableExt.ConsoleTableBuilder
                .From(tableData)
                .WithColumn("Id", "Session Start", "Session End", "Duration")
                .ExportAndWriteLine();
        }

        /// <summary>
        /// Sums the total of durations of the sessions contained in supplied session list.
        /// </summary>
        /// <param name="sessionList">List of Sessions</param>
        /// <returns>TimeSpan total in format DD:HH:MM:SS</returns>
        public static TimeSpan TotalHoursReport(List<Session> sessionList)
        {
            TimeSpan total = TimeSpan.Zero;

            foreach (Session session in sessionList)
            {
                total += session.Duration;
            }
            TimeSpan cleanTotal = RemoveMilliseconds(total);
            return cleanTotal;
        }

        /// <summary>
        /// Removes millisecond alement from a timespan.
        /// </summary>
        /// <param name="time">TimeSpan to clean.</param>
        /// <returns>TimeSpan in format DD:HH:MM:SS</returns>
        public static TimeSpan RemoveMilliseconds(TimeSpan time)
        {
            return new TimeSpan(time.Days, time.Hours, time.Minutes, time.Seconds);
        }
    }
}
