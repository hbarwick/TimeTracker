using ConsoleTableExt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker
{
    public class ReportGeneration
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

    }
}
