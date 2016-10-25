using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

/* GENERAL NOTES
 * 1) References in use - Assembly-CSharp.dll, Assembly-CSharp-firstpass.dll, UnityEngine.dll and UnityEngine.UI.dll
 * 2) GPL 2.0 so go nuts with this project!
 */

namespace commnetpeek
{
    // Called when you are in the flight scene
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class CommNetPeek: MonoBehaviour
    {
        public void Start()
        {
            CNPLog.Verbose("Flight script starts");
        }

        public void OnDestroy()
        {
            CNPLog.Verbose("Flight script ends");
        }

        public void Awake()
        {
            CNPLog.Verbose("Flight script awakes");
        }

        public void Update()
        {
            //CNPLog.Verbose("Flight script updates");
        }

        public void OnGUI()
        {
            //CNPLog.Verbose("Flight script does GUI stuff");
        }
    }
}
