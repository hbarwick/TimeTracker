using ConsoleTableExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker
{
    public class Reports
    {
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

        public static TimeSpan RemoveMilliseconds(TimeSpan time)
        {
            return new TimeSpan(time.Days, time.Hours, time.Minutes, time.Seconds);
        }
    }
}
