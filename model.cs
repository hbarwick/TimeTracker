﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingTracker
{
    internal class Session
    {
        public int Id { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public TimeSpan SessionDuration
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