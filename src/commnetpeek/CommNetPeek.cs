using UnityEngine;

/* GENERAL NOTES
 * 1) References in use - Assembly-CSharp.dll, Assembly-CSharp-firstpass.dll, UnityEngine.dll and UnityEngine.UI.dll
 * 2) Compiled against Net 3.5
 * 3) GPL 2.0 so go nuts with this project!
 */

//TODO: Signal delay

namespace commnetpeek
{
    //This class is coupled with the commnetpeek_module.cfg
    public class CommNetPeekModule : PartModule
    {
        [KSPEvent(guiActive = true, guiActiveUnfocused = true, guiName = "CommNet Peek", active = true)]
        public void KSPEventPeek()
        {
            string vesselName = part.vessel.GetName(); // don't use FlightGlobals.ActiveVessel. Could open from another nearby vessel or EVA
            string partType = part.partInfo.title;

            SimpleOutputDialog debug_window = new SimpleOutputDialog("CommNet Peek", "Simple output from Part '"+partType+"' of Vessel '"+vesselName+"'.");
            debug_window.launch(part.vessel, part);
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
    }
}
