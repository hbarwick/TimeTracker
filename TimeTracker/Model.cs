namespace TimeTracker
{
    /// <summary>
    /// Class <c>Session</c> models a session of time. Supply with DateTime <value name="StartTime"/> and <value name="EndTime"/>,
    /// Timespan <value name="Duration"/> will be auto calculated.
    /// </summary>
    public class Session
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public TimeSpan Duration
        {
            get
            {
                if (EndTime == null)
                {
                    return TimeSpan.Zero;
                }
                else
                {
                    return (TimeSpan)(EndTime - StartTime);
                }
            }
        }
    }
}
