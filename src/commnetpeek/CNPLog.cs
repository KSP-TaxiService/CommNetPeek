using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* Attributes to be display in a debug window are defined in commnetpeek_settings.cfg
 * 
 *
 */

namespace commnetpeek
{
    public static class CNPLog
    {
        public static readonly string NAME_LOG_PREFIX = "CommNet Peek";

        static CNPLog()
        {
            //Empty
        }

        public static void Verbose(string message)
        {
            UnityEngine.Debug.Log(NAME_LOG_PREFIX + " -> verbose: " + message);
        }

        public static void Debug(string message)
        {
            UnityEngine.Debug.Log(NAME_LOG_PREFIX + " -> debug: " + message);
        }
    }
}
