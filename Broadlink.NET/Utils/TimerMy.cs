using System;

namespace Broadlink.NET
{
    public class TimerMy : System.Timers.Timer
    {
        public enum Froms
        {
            FromMilliseconds,
            FromSeconds,
            FromMinutes,
            FromHours,
            FromDays,
            FromTicks
        }
        public TimerMy() : this(100, Froms.FromMilliseconds)
        {

        }

        public TimerMy(double interval, Froms from = Froms.FromSeconds)
        {
            Enabled = false;
            AutoReset = true;
            switch (from)
            {
                case Froms.FromMilliseconds:
                    Interval = interval;
                    break;
                case Froms.FromSeconds:
                    Interval = TimeSpan.FromSeconds(interval).TotalMilliseconds;
                    break;
                case Froms.FromMinutes:
                    Interval = TimeSpan.FromMinutes(interval).TotalMilliseconds;
                    break;
                case Froms.FromHours:
                    Interval = TimeSpan.FromHours(interval).TotalMilliseconds;
                    break;
                case Froms.FromDays:
                    Interval = TimeSpan.FromDays(interval).TotalMilliseconds;
                    break;
                case Froms.FromTicks:
                    Interval = TimeSpan.FromTicks(Convert.ToInt64(interval)).TotalMilliseconds;
                    break;
            }
        }
    }
}
