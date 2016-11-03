using System;
using System.Collections.Generic;
using System.Linq;

using CommNet;
using UnityEngine;
using UnityEngine.UI;

namespace commnetpeek
{
    public class SimpleOutputDialog : UI.AbstractDialog
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
            DialogGUIBase[] messageLabels = new DialogGUIBase[eachMessageGroupList.Count + 1];
            messageLabels[0] = new DialogGUIContentSizer(ContentSizeFitter.FitMode.Unconstrained, ContentSizeFitter.FitMode.PreferredSize, true);
            for (int i = 0; i < eachMessageGroupList.Count; i++)
                messageLabels[i+1] = eachMessageGroupList[i];

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

            processSignalPath(thisVessel, commandPart);

            processNeighbourNodes(thisVessel, commandPart, netInstance);

            //RemoteTech info
            messages.Enqueue(stringNewline + stringTitle("RemoteTech"));
            messages.Enqueue("Not implemented yet");

            //Stock CommNet's debug
            messages.Enqueue(stringNewline + stringTitle("CommNetwork.CreateDebug()"));
            messages.Enqueue(netInstance.CreateDebug());

            return true;
        }

        private string stringTitle(string title)
        {
            return stringSeparator + stringNewline +
                   "- " + title + stringNewline + 
                   stringSeparator;
        }

        private void processSignalPath(Vessel thisVessel, Part commandPart)
        {
            CommNetVessel commVesselInfo = thisVessel.connection;
            CommPath graphPath = commVesselInfo.ControlPath;
            List<CommLink> edges = graphPath.ToList();
            messages.Enqueue(string.Format("{0}Signal path: {1}", stringNewline, edges.Count() == 0 ? "Non-existent" : ""));

            double totalDistanceCost = 0;
            bool firstNodeProcessed = false;
            for (int i = 0; i < edges.Count(); i++)
            {
                CommLink thisEdge = edges.ElementAt(i);
                totalDistanceCost += thisEdge.cost;

                if (!firstNodeProcessed)
                {
                    messages.Enqueue(string.Format("{0}1) PART - {1} @ {2}", stringTab, commandPart.partInfo.title, commVesselInfo.Vessel.mainBody.bodyName));
                    firstNodeProcessed = true;
                }

                CommNode destNode = thisEdge.b;
                Vessel destVessel = CNPUtils.findCorrespondingVessel(destNode);

                string nodeType;
                string nodeLocation;
                string nodeName = destNode.name;
                double signalStrength = thisEdge.signalStrength;

                if (destVessel == null) //could be a celestial body 
                    nodeLocation = FlightGlobals.GetHomeBodyName();
                else
                    nodeLocation = destVessel.mainBody.bodyName;

                if (thisEdge.hopType == HopType.ControlPoint)
                    nodeType = "REMOTE PILOT";
                else if (thisEdge.hopType == HopType.Home)
                    nodeType = "MISSION CONTROL";
                else
                    nodeType = "RELAY";

                messages.Enqueue(string.Format("{0}{1}) {2} - {3} @ {4} (signal {5}%)", stringTab, i + 2, nodeType, CNPUtils.neatVesselName(nodeName), nodeLocation, CNPUtils.neatSignalStrength(signalStrength)));
            }
            messages.Enqueue(string.Format("{0}Distance: {1} (+{2:0.##}s)", stringTab, CNPUtils.neatDistance(totalDistanceCost), totalDistanceCost / 3E+08));
        }

        private void processNeighbourNodes(Vessel thisVessel, Part commandPart, CommNetwork netInstance)
        {
            CommNetVessel commVesselInfo = thisVessel.connection;

            //return net.FindPath(vessel1.Connection.Comm, tempPath, vessel2.Connection.Comm) || net.FindPath(vessel2.Connection.Comm, tempPath, vessel1.Connection.Comm);

            //public override CommNode FindClosestWhere(CommNode start, CommPath path, Func<CommNode, CommNode, bool> where);
        

            messages.Enqueue(string.Format("{0}Neighbour nodes: {1} node(s)", stringNewline, 3));
            //messages.Enqueue(stringTab + "3) 13Mm to VESSEL3 @ Minus (signal %100)");
        }
    }
}
