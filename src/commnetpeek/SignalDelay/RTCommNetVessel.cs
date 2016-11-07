using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CommNet;

/* One idea: AT the start of flight scene, 
 * 1) get reference to the active vessel
 * 2) examine vessel's connection instance
 * 3) if that instance is null, give it RTCommNetVessel instance
 * 4) if that instance is not null, copy its variable values into RTCommNetVessel instance and replace the prev instance
 */

namespace commnetpeek.SignalDelay
{
    public class RTCommNetVessel : CommNetVessel
    {
        private SignalDelayCore signalCore;

        public RTCommNetVessel()
        {
            this.signalCore = new SignalDelayCore();
            updateSignalDelay();
        }

        protected void Start()
        {
            CNPLog.Debug("RTCommNetVessel.Start() @ "+this.Vessel.GetName());
            base.Start();
        }

        public void updateSignalDelay()
        {
            base.signalDelay = signalCore.computeSignalDelay(base.controlPath);
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

        protected override void OnNetworkInitialized()
        {
            CNPLog.Debug("OnNetworkInitialized");
            base.OnNetworkInitialized();
        }

        protected override void Update()
        {
            //CNPLog.Debug("Update");
            base.Update();
        }

        protected override void UpdateComm()
        {
            CNPLog.Debug("UpdateComm");
            base.UpdateComm();
        }
    }
}
