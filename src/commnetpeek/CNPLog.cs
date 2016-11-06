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

        public static void Verbose(string message, params object[] param)
        {
            UnityEngine.Debug.Log(string.Format("{0} -> verbose: {1}", NAME_LOG_PREFIX, string.Format(message, param)));
        }

        public static void Debug(string message, params object[] param)
        {
            UnityEngine.Debug.LogWarning(string.Format("{0} -> debug: {1}", NAME_LOG_PREFIX, string.Format(message, param)));
        }

        public static void Error(string message, params object[] param)
        {
            UnityEngine.Debug.LogError(string.Format("{0} -> ERROR: {1}", NAME_LOG_PREFIX, string.Format(message, param)));
        }
    }
}
