using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CommNet;

/* Notes and observations:
 * 1) RTCommNetVessel is automatically accepted and attached to every existing vessel by KSP
 * 2) Vessel's connection variable is RTCommNetVessel class instead of CommNetVessel
 * 3) For the RemoteTech 2.0, the RTCommNetVessel class should be the single point of various major components
 *    such as the signal delay and flight computer
 */

namespace commnetpeek.SignalDelay
{
    public class RTCommNetVessel : CommNetVessel
    {
        private static SignalDelayCore signalCore;
        private Vessel motherVessel;

        //-----------
        // Automatic events
        //-----------
        [KSPField(guiActive = true, guiName = "Hunger", guiFormat = "#0.00\\%"), UI_ProgressBar(scene = UI_Scene.Flight, minValue = 0f, maxValue = 100f)]
        public float value = 50f;

        protected override void OnNetworkInitialized()
        {
            CNPLog.Debug("RTCommNetVessel.OnNetworkInitialized() @ " + this.Vessel.GetName());
            RemoteTechStartUp();
            base.OnNetworkInitialized();
        }

        public override void OnNetworkPostUpdate()
        {
            //CNPLog.Debug("OnNetworkPostUpdate");
            base.OnNetworkPostUpdate();
        }

        public override void OnNetworkPreUpdate()
        {
            //CNPLog.Debug("OnNetworkPreUpdate");
            base.OnNetworkPreUpdate();
        }

        protected override void UpdateComm()
        {
            //CNPLog.Debug("UpdateComm");

            updateSignalDelay();

            base.UpdateComm();
        }

        //-----------
        // RemoteTech methods
        //-----------
        private void RemoteTechStartUp()
        {
            if(signalCore == null)
                signalCore = new SignalDelayCore();

            motherVessel = this.Vessel;
        }

        public void updateSignalDelay()
        {
            base.signalDelay = signalCore.computeSignalDelay(base.controlPath);
            //signalDelayString = base.signalDelay;
        }
    }
}

