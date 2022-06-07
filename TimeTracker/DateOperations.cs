namespace TimeTracker
{
    internal class DateOperations
    {
        /// <summary>
        /// Prompts user for text input and parses to datetime, reprompting if unable to parse.
        /// </summary>
        /// <returns>Date portion as string in ShortDateString format: YYYY-MM-DD</returns>
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

        /// <summary>
        /// Prompts user for text input and parses to datetime, reprompting if unable to parse.
        /// </summary>
        /// <returns>Time portion as string in ShortTimeString format: HH:MM:SS</returns>
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

        /// <summary>
        /// Parses date string and time string together into a DateTime object.
        /// </summary>
        /// <param name="date">Date string</param>
        /// <param name="time">Time String</param>
        /// <returns>DateTime object</returns>
        public static DateTime ParseDateTime(string date, string time)
        {
            string dt = date + " " + time;
            DateTime parsedDt = DateTime.Parse(dt);
            return parsedDt;
        }

        /// <summary>
        /// Get todays date as string in format YYYY-MM-DD.
        /// </summary>
        /// <returns>date string YYYY-MM-DD</returns>
        public static string GetTodaysDate()
        {
            Console.WriteLine("Getting today's date....");
            DateTime dateTime = DateTime.Now;
            string date = dateTime.ToShortDateString();
            return date;
        }

        /// <summary>
        /// Returns the first and last date of the month, defaulting to current month
        /// </summary>
        /// <param name="current">Optional parameter (default 0), deducts a month. E.G pass 1 to 
        /// return previos month's start and end date</param>
        /// <returns>Packaged Tuple of 2xDateTime objects</returns>
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
