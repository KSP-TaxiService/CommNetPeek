using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CommNet;

namespace commnetpeek.SignalDelay
{
    public class SignalDelayCore
    {
        private Settings CNPSettings;
        private Queue<AbstractCommand> commandQueue;

        public SignalDelayCore()
        {
            this.CNPSettings = CommNetPeekSettings.Instance;
            this.commandQueue = new Queue<AbstractCommand>();
        }

        public bool isEnabled()
        {
            return CNPSettings.EnableSignalDelay;
        }

        public double computeSignalDelay(CommPath controlPath)
        {
            if (!CNPSettings.EnableSignalDelay || controlPath == null)
                return 0.0;

            double totalDistanceCost = 0.0;
            for (int i = 0; i < controlPath.Count(); i++)
            {
                totalDistanceCost += controlPath.ElementAt(i).cost;
            }

            return totalDistanceCost / CNPSettings.SpeedOfLight;
        }

        public void enqueueCommand(AbstractCommand pendingCommand, double signalDelay)
        {
            commandQueue.Enqueue(pendingCommand);
        }

        //TODO: how to design a way to dispatch the pending command back to its caller method when its countdown reaches zero?
    }
}
