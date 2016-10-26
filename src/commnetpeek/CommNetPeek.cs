using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

/* GENERAL NOTES
 * 1) References in use - Assembly-CSharp.dll, Assembly-CSharp-firstpass.dll, UnityEngine.dll and UnityEngine.UI.dll
 * 2) GPL 2.0 so go nuts with this project!
 */

/* TO-DO list
 * 1) Find out about PopupDialog's blocking mechanism on right-click context menu
 * 2) Disable the above mechanism when dialog is active
 * 3) Signal delay
 * 4) Issue with positioning the dialog
 */ 

namespace commnetpeek
{
    //load from setting file
    public static class CommNetPeekSettings
    {
        private static bool isLoaded = false;
        public static void loadFromFile()
        {
            string filename = "commnetpeek_settings.cfg";
            isLoaded = true;
        }
    }

    //This class is coupled with the commnetpeek_module.cfg
    public class CommNetPeekModule : PartModule
    {
        [KSPEvent(guiActive = true, guiActiveUnfocused = true, guiName = "CommNet Peek", active = true)]
        public void KSPEventPeek()
        {
            string vesselName = FlightGlobals.ActiveVessel.GetName();
            string partType = part.partInfo.title;

            SimpleOutputDialog debug_window = new SimpleOutputDialog("CommNet Peek","Simple output from Part '"+partType+"' of Vessel '"+vesselName+"'.");
            debug_window.launch(FlightGlobals.ActiveVessel);
        }
    }

    // Called when you are in the flight scene
    [KSPAddon(KSPAddon.Startup.Flight, false)]
    public class CommNetPeek: MonoBehaviour
    {
        public void Start()
        {
            CNPLog.Verbose("Flight script starts");
            CommNetPeekSettings.loadFromFile();
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
