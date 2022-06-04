using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker
{
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
