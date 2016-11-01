using System;
using System.Collections.Generic;
using System.Linq;

using CommNet;
using UnityEngine;
using UnityEngine.UI;

namespace commnetpeek
{
    public class SimpleOutputDialog : UI.AbstractDebugDialog
    {
        private string briefMessage;
        private Queue<string> messages;

        //Quick string additions
        public static readonly string stringSeparator = new String('-', 30);
        public static readonly string stringNewline = "\n";
        public static readonly string stringTab = "\t";

        public SimpleOutputDialog(string title, string briefMessage) : base(title, 
                                                                            0.2f,                        //x
                                                                            0.5f,                        //y
                                                                            (int)(Screen.width*0.3),     //width
                                                                            (int)(Screen.height* 0.4))   //height
        {
            this.briefMessage = briefMessage;
            messages = new Queue<string>();
        }

        protected override List<DialogGUIBase> drawContentComponents()
        {
            List<DialogGUIBase> listComponments = new List<DialogGUIBase>();

            //Label for the brief message
            listComponments.Add(new DialogGUIHorizontalLayout(true, false, 0, new RectOffset(), TextAnchor.UpperCenter, new DialogGUIBase[] { new DialogGUILabel(this. briefMessage, false, false) }));

            //A GUILayout for each message (eg label + textfield + button)
            List<DialogGUIHorizontalLayout> eachMessageGroupList = new List<DialogGUIHorizontalLayout>();
            while(messages.Count > 0)
            {
                DialogGUILabel messageLabel = new DialogGUILabel(messages.Dequeue(), false, false);
                DialogGUIHorizontalLayout lineGroup = new DialogGUIHorizontalLayout(true, false, 4, new RectOffset(), TextAnchor.UpperLeft, new DialogGUIBase[] { messageLabel });
                eachMessageGroupList.Add(lineGroup);
            }

            //Prepare a list container for the message GUILayouts
            DialogGUIBase[] messageLabels = new DialogGUIBase[eachMessageGroupList.Count];
            for (int i = 0; i < eachMessageGroupList.Count; i++)
                messageLabels[i] = eachMessageGroupList[i];

            listComponments.Add(new DialogGUIScrollList(Vector2.one, false, true, new DialogGUIVerticalLayout(10, 100, 4, new RectOffset(5, 15, 0, 0), TextAnchor.UpperLeft, messageLabels)));
            
            return listComponments;
        }

        protected override bool runIntenseInfo(Vessel thisVessel, Part commandPart)
        {
            CommNetwork netInstance = CommNetNetwork.Instance.CommNet;
            CommNetVessel commVesselInfo = thisVessel.connection;
            
            //Connection info
            messages.Enqueue(stringTitle("Connection"));

            //quick glance
            messages.Enqueue(string.Format("Control status: {0} (signal {1}%)", commVesselInfo.ControlState, CNPUtils.neatSignalStrength(commVesselInfo.SignalStrength)));
            messages.Enqueue(string.Format("Vessel status: {0}", thisVessel.situation));
            messages.Enqueue(string.Format("Signal delay (s): {0}", commVesselInfo.SignalDelay));

            //signal path
            CommPath graphPath = commVesselInfo.ControlPath;
            double totalDistanceCost = 0;
            messages.Enqueue(string.Format("{0}Signal path:", stringNewline));

            bool firstNodeProcessed = false;
            List<CommLink> edges = graphPath.ToList();
            for (int i=0; i<edges.Count(); i++)
            {
                CommLink thisEdge = edges.ElementAt(i);
                totalDistanceCost += thisEdge.cost;

                if(!firstNodeProcessed)
                {
                    messages.Enqueue(string.Format("{0}1) YOU @ {1}", stringTab, commVesselInfo.Vessel.lastBody.bodyName));
                    firstNodeProcessed = true;
                }

                CommNode destNode = thisEdge.b;
                Vessel destVessel = CNPUtils.findCorrespondingVessel(destNode);

                string commType;
                string location;
                string nodeName = destNode.name;
                double signalStrength = thisEdge.signalStrength;

                if (destVessel == null) //could be a celestial body 
                    location = FlightGlobals.GetHomeBodyName();
                else
                    location = destVessel.mainBody.bodyName;

                if (destNode.isControlSourceMultiHop)
                    commType = "KCS";
                else
                    commType = "RELAY"; // what about pilot?

                messages.Enqueue(string.Format("{0}{1}) {2} - {3} @ {4} (signal {5}%)", stringTab, i+2, commType, CNPUtils.neatVesselName(nodeName), location, CNPUtils.neatSignalStrength(signalStrength)));
            }
            messages.Enqueue(string.Format("{0}Distance: {1} (+{2:0.##}s)", stringTab, CNPUtils.neatDistance(totalDistanceCost), totalDistanceCost/3E+08));

            //nearest neighbour nodes
            messages.Enqueue(string.Format("{0}Nearest neighbours (excluded the one in the signal path): {1} node(s)", stringNewline, 3));
            messages.Enqueue(stringTab + "1) 13k to VESSEL1 @ Kerbin (signal %2)");
            messages.Enqueue(stringTab + "2) 2Mm to VESSEL2 @ Mun (signal %100)");
            messages.Enqueue(stringTab + "3) 13Mm to VESSEL3 @ Minus (signal %100)");

            //RemoteTech info
            messages.Enqueue(stringNewline + stringTitle("RemoteTech"));
            messages.Enqueue("Not implemented");

            //Stock CommNet's debug
            /*
            messages.Enqueue(stringTitle("CommNetwork.CreateDebug()"));
            string[] lines = netInstance.CreateDebug().Split('\n');
            for (int i = 0; i < lines.Length; i++)
                messages.Enqueue(lines[i]);
            */

            return true;
        }

        private string stringTitle(string title)
        {
            return stringSeparator + stringNewline +
                   "- " + title + stringNewline + 
                   stringSeparator;
        }
    }
}
