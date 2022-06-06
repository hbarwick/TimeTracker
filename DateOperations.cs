using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker
{
    internal class DateOperations
    {
        public static string EnterNewDate()
        {
            Console.Write("\nEnter date: ");
            string? DateEntry = Console.ReadLine();
            DateTime Date;
            while (!DateTime.TryParse(DateEntry, out Date))
            {
                Console.Write("\nEnter date: ");
                DateEntry = Console.ReadLine();
            }
            string shortDate = Date.ToShortDateString();
            return shortDate;
        }

        public static string EnterNewTime()
        {
            Console.Write("\nEnter time: ");
            string? TimeEntry = Console.ReadLine();
            DateTime Time;
            while (!DateTime.TryParse(TimeEntry, out Time))
            {
                Console.Write("\nEnter date: ");
                TimeEntry = Console.ReadLine();
            }
            string shortTime = Time.ToShortTimeString();
            return shortTime;
        }

        public static DateTime ParseDateTime(string date, string time)
        {
            string dt = date + " " + time;
            DateTime parsedDt = DateTime.Parse(dt);
            return parsedDt;
        }

        public static string GetTodaysDate()
        {
            Console.WriteLine("Getting today's date....");
            DateTime dateTime = DateTime.Now;
            string date = dateTime.ToShortDateString();
            return date;
        }

        public static (DateTime, DateTime) GetFirstAndLastOfMonth(int current = 0)
        {
            int currentMonth = DateTime.Now.Month - current;
            int currentYear = DateTime.Now.Year;
            var firstOfMonth = new DateTime(currentYear, currentMonth, 1);
            var lastOfMonth = firstOfMonth.AddMonths(1).AddSeconds(-1);
            return (firstOfMonth, lastOfMonth);
        }
    }
}
