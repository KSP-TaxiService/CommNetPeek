using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace commnetpeek.SignalDelay
{
    //Temporary class until the RemoteTech's AbstractCommand class takes over this
    public abstract class AbstractCommand
    {
        public double TimeStamp;
        public double ExtraDelay;
        public double Delay { get { return Math.Max(TimeStamp - Planetarium.GetUniversalTime(), 0); } }

        public String Description
        {
            get
            {
                double delay = this.Delay;
                if (delay > 0 || ExtraDelay > 0)
                {
                    string extra = (ExtraDelay > 0) ? string.Format("{0} + {1}", delay, ExtraDelay) : ""+delay;
                    return "Signal delay: " + extra;
                }
                return "";
            }
        }
    }
}
